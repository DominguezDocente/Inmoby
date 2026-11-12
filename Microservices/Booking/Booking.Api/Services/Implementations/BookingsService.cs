using Booking.Api.Core;
using Booking.Api.Core.Pagination;
using Booking.Api.Data;
using Booking.Api.Data.Entities;
using Booking.Api.DTOs.Booking;
using Booking.Api.Services.Abstractions;
using Booking.Api.Services.External.Abstractions;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Booking.Api.Data.Entities.Booking;

namespace Booking.Api.Services.Implementations
{
    public class BookingsService : IBookingsService
    {
        private const int MinimumMinutesBeforeVisit = 5;

        private readonly DataContext _context;
        private readonly IPropertiesClient _propertiesClient;

        public BookingsService(DataContext context, IPropertiesClient propertiesClient)
        {
            _context = context;
            _propertiesClient = propertiesClient;
        }

        public async Task<Response<BookingDTO>> CreateAsync(Guid userId, CreateBookingDTO dto)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return Response<BookingDTO>.Failure("No se pudo identificar al usuario autenticado.",
                                                        statusCode: StatusCodes.Status401Unauthorized);
                }

                if (dto.PropertyId == Guid.Empty)
                {
                    return Response<BookingDTO>.Failure("El inmueble es requerido.");
                }

                Response<BookingDTO>? propertyValidation = await ValidatePropertyAsync(dto.PropertyId);

                if (propertyValidation is not null)
                {
                    return propertyValidation;
                }

                DateTime scheduledAt = NormalizeToUtc(dto.ScheduledAt);

                Response<BookingDTO>? validation = await ValidateScheduleAsync(dto.PropertyId,
                                                                               userId,
                                                                               scheduledAt,
                                                                               dto.DurationMinutes);

                if (validation is not null)
                {
                    return validation;
                }

                BookingEntity booking = new()
                {
                    Id = Guid.CreateVersion7(),
                    PropertyId = dto.PropertyId,
                    UserId = userId,
                    ScheduledAt = scheduledAt,
                    DurationMinutes = dto.DurationMinutes,
                    Status = BookingStatus.Pending,
                    Notes = NormalizeText(dto.Notes),
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Bookings.AddAsync(booking);
                await _context.SaveChangesAsync();

                return Response<BookingDTO>.Success(ToDto(booking), "Visita agendada con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response<BookingDTO>> GetOneAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.AsNoTracking()
                                                           .FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response<BookingDTO>.NotFound($"No existe una visita con id {id}.");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Response<BookingDTO>.Forbidden();
                }

                return Response<BookingDTO>.Success(ToDto(booking), "Visita obtenida con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<BookingDTO>>> GetPaginationAsync(BookingPaginationRequest request,
                                                                                       Guid userId,
                                                                                       bool isAdmin)
        {
            try
            {
                IQueryable<BookingEntity> query = _context.Bookings.AsNoTracking();

                if (!isAdmin)
                {
                    query = query.Where(b => b.UserId == userId);
                }
                else if (request.UserId.HasValue)
                {
                    query = query.Where(b => b.UserId == request.UserId.Value);
                }

                if (request.Status.HasValue)
                {
                    query = query.Where(b => b.Status == request.Status.Value);
                }

                if (request.PropertyId.HasValue)
                {
                    query = query.Where(b => b.PropertyId == request.PropertyId.Value);
                }

                if (request.From.HasValue)
                {
                    DateTime from = NormalizeToUtc(request.From.Value);
                    query = query.Where(b => b.ScheduledAt >= from);
                }

                if (request.To.HasValue)
                {
                    DateTime to = NormalizeToUtc(request.To.Value);
                    query = query.Where(b => b.ScheduledAt <= to);
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    string filter = request.Filter.Trim().ToLower();
                    query = query.Where(b => b.Notes != null && b.Notes.ToLower().Contains(filter));
                }

                query = query.OrderBy(b => b.ScheduledAt);

                PagedList<BookingEntity> list = await PagedList<BookingEntity>.ToPagedListAsync(query, request);
                List<BookingDTO> items = list.Select(ToDto).ToList();

                PaginationResponse<BookingDTO> response = new()
                {
                    List = new PagedList<BookingDTO>(items, list.TotalCount, list.CurrentPage, list.RecordsPerPage),
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    RecordsPerPage = list.RecordsPerPage,
                    TotalCount = list.TotalCount,
                    Filter = request.Filter
                };

                return Response<PaginationResponse<BookingDTO>>.Success(response, "Visitas obtenidas con éxito.");
            }
            catch (Exception ex)
            {
                return Response<PaginationResponse<BookingDTO>>.Failure(ex);
            }
        }

        public async Task<Response<BookingDTO>> UpdateAsync(Guid id,
                                                            UpdateBookingDTO dto,
                                                            Guid userId,
                                                            bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response<BookingDTO>.NotFound($"No existe una visita con id {id}.");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Response<BookingDTO>.Forbidden();
                }

                if (booking.Status is BookingStatus.Cancelled or BookingStatus.Completed)
                {
                    return Response<BookingDTO>.Failure("No se puede reprogramar una visita cancelada o completada.");
                }

                Response<BookingDTO>? propertyValidation = await ValidatePropertyAsync(booking.PropertyId);

                if (propertyValidation is not null)
                {
                    return propertyValidation;
                }

                DateTime scheduledAt = NormalizeToUtc(dto.ScheduledAt);

                Response<BookingDTO>? validation = await ValidateScheduleAsync(booking.PropertyId,
                                                                               booking.UserId,
                                                                               scheduledAt,
                                                                               dto.DurationMinutes,
                                                                               booking.Id);

                if (validation is not null)
                {
                    return validation;
                }

                booking.ScheduledAt = scheduledAt;
                booking.DurationMinutes = dto.DurationMinutes;
                booking.Notes = NormalizeText(dto.Notes);
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Response<BookingDTO>.Success(ToDto(booking), "Visita actualizada con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response<BookingDTO>> ConfirmAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response<BookingDTO>.NotFound($"No existe una visita con id {id}.");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Response<BookingDTO>.Forbidden();
                }

                if (booking.Status == BookingStatus.Confirmed)
                {
                    return Response<BookingDTO>.Success(ToDto(booking), "La visita ya estaba confirmada.");
                }

                if (booking.Status != BookingStatus.Pending)
                {
                    return Response<BookingDTO>.Failure("Solo se pueden confirmar visitas pendientes.");
                }

                booking.Status = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Response<BookingDTO>.Success(ToDto(booking), "Visita confirmada con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response<BookingDTO>> CancelAsync(Guid id,
                                                            CancelBookingDTO dto,
                                                            Guid userId,
                                                            bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response<BookingDTO>.NotFound($"No existe una visita con id {id}.");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Response<BookingDTO>.Forbidden();
                }

                if (booking.Status == BookingStatus.Cancelled)
                {
                    return Response<BookingDTO>.Success(ToDto(booking), "La visita ya estaba cancelada.");
                }

                if (booking.Status == BookingStatus.Completed)
                {
                    return Response<BookingDTO>.Failure("No se puede cancelar una visita completada.");
                }

                booking.Status = BookingStatus.Cancelled;
                booking.CancelledAt = DateTime.UtcNow;
                booking.CancelReason = NormalizeText(dto.Reason);
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Response<BookingDTO>.Success(ToDto(booking), "Visita cancelada con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response<BookingDTO>> CompleteAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response<BookingDTO>.NotFound($"No existe una visita con id {id}.");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Response<BookingDTO>.Forbidden();
                }

                if (booking.Status == BookingStatus.Completed)
                {
                    return Response<BookingDTO>.Success(ToDto(booking), "La visita ya estaba completada.");
                }

                if (booking.Status != BookingStatus.Confirmed)
                {
                    return Response<BookingDTO>.Failure("Solo se pueden completar visitas confirmadas.");
                }

                booking.Status = BookingStatus.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Response<BookingDTO>.Success(ToDto(booking), "Visita completada con éxito.");
            }
            catch (Exception ex)
            {
                return Response<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Response.NotFound($"No existe una visita con id {id}.");
                }

                _context.Remove(booking);
                await _context.SaveChangesAsync();

                return Response.Success("Visita eliminada con éxito.");
            }
            catch (Exception ex)
            {
                return Response.Failure(ex);
            }
        }

        private async Task<Response<BookingDTO>?> ValidatePropertyAsync(Guid propertyId)
        {
            PropertyValidationResult validation = await _propertiesClient.ValidateAsync(propertyId);

            return validation.Status switch
            {
                PropertyValidationStatus.Available => null,
                PropertyValidationStatus.NotFound => Response<BookingDTO>.NotFound("El inmueble no existe."),
                PropertyValidationStatus.Unavailable => Response<BookingDTO>.Failure("El inmueble no está disponible para visitas."),
                PropertyValidationStatus.Unauthorized => Response<BookingDTO>.Failure("No fue posible validar el inmueble por un problema de permisos.",
                                                                                        statusCode: StatusCodes.Status502BadGateway),
                _ => Response<BookingDTO>.Failure("No se pudo validar el inmueble. Intenta nuevamente.",
                                                  statusCode: StatusCodes.Status503ServiceUnavailable)
            };
        }

        private async Task<Response<BookingDTO>?> ValidateScheduleAsync(Guid propertyId,
                                                                       Guid userId,
                                                                       DateTime scheduledAt,
                                                                       int durationMinutes,
                                                                       Guid? excludeBookingId = null)
        {
            if (scheduledAt <= DateTime.UtcNow.AddMinutes(MinimumMinutesBeforeVisit))
            {
                return Response<BookingDTO>.Failure($"La visita debe agendarse con al menos {MinimumMinutesBeforeVisit} minutos de anticipación.");
            }

            if (durationMinutes is < 15 or > 240)
            {
                return Response<BookingDTO>.Failure("La duración debe estar entre 15 y 240 minutos.");
            }

            bool propertyHasOverlap = await HasPropertyOverlapAsync(propertyId,
                                                                    scheduledAt,
                                                                    durationMinutes,
                                                                    excludeBookingId);

            if (propertyHasOverlap)
            {
                return Response<BookingDTO>.Failure("El inmueble ya tiene una visita agendada en ese horario.");
            }

            bool userHasOverlap = await HasUserOverlapAsync(userId,
                                                            scheduledAt,
                                                            durationMinutes,
                                                            excludeBookingId);

            if (userHasOverlap)
            {
                return Response<BookingDTO>.Failure("Ya tienes otra visita agendada en ese horario.");
            }

            return null;
        }

        private async Task<bool> HasPropertyOverlapAsync(Guid propertyId,
                                                         DateTime scheduledAt,
                                                         int durationMinutes,
                                                         Guid? excludeBookingId)
        {
            DateTime end = scheduledAt.AddMinutes(durationMinutes);

            IQueryable<BookingEntity> query = _context.Bookings.Where(b =>
                b.PropertyId == propertyId &&
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) &&
                b.ScheduledAt < end &&
                b.ScheduledAt.AddMinutes(b.DurationMinutes) > scheduledAt);

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }

        private async Task<bool> HasUserOverlapAsync(Guid userId,
                                                     DateTime scheduledAt,
                                                     int durationMinutes,
                                                     Guid? excludeBookingId)
        {
            DateTime end = scheduledAt.AddMinutes(durationMinutes);

            IQueryable<BookingEntity> query = _context.Bookings.Where(b =>
                b.UserId == userId &&
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) &&
                b.ScheduledAt < end &&
                b.ScheduledAt.AddMinutes(b.DurationMinutes) > scheduledAt);

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }

        private static DateTime NormalizeToUtc(DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
            };
        }

        private static string? NormalizeText(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static BookingDTO ToDto(BookingEntity booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                PropertyId = booking.PropertyId,
                UserId = booking.UserId,
                ScheduledAt = booking.ScheduledAt,
                DurationMinutes = booking.DurationMinutes,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt,
                CancelledAt = booking.CancelledAt,
                CancelReason = booking.CancelReason
            };
        }
    }
}
