using Booking.Api.Core;
using Booking.Api.Core.Extensions;
using Booking.Api.Core.Pagination;
using Booking.Api.Data;
using Booking.Api.Data.Entities;
using Booking.Api.DTOs.Booking;
using Booking.Api.Services.Abstractions;
using Booking.Api.Services.External.Abtractions;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Booking.Api.Data.Entities.Booking;

namespace Booking.Api.Services.Implemetations
{
    public class BookingService : IBookingService
    {
        private const int MinimumMinutesBeforeVisit = 5;

        private readonly DataContext _context;
        private readonly IPropertiesClient _propertiesClient;

        public BookingService(DataContext context, IPropertiesClient propertiesClient)
        {
            _context = context;
            _propertiesClient = propertiesClient;
        }

        public async Task<Result<BookingDTO>> CancelAsync(Guid id, Guid userId, CancelBookingDTO dto, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Result<BookingDTO>.Failure($"No existe visita con id '{id}'");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Result<BookingDTO>.Failure("NO tiene permiso para visualizar esta visita");
                }

                if (booking.Status == BookingStatusEnum.Cancelled)
                {
                    return Result<BookingDTO>.Success(ToDTO(booking), "la visita ya estaba cancelada");
                }

                if (booking.Status == BookingStatusEnum.Completed)
                {
                    return Result<BookingDTO>.Failure("NO puede cancelar una visita completada");
                }

                booking.Status = BookingStatusEnum.Cancelled;
                booking.CancelledAt = DateTime.UtcNow;
                booking.CancelReason = dto.Reason;
                booking.UpdatedAt = DateTime.UtcNow;

                _context.Bookings.Update(booking);

                await _context.SaveChangesAsync();

                return Result<BookingDTO>.Success(ToDTO(booking), "Visita cancelada ocn éxito");
            }
            catch(Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Result<BookingDTO>> CompleteAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Result<BookingDTO>.Failure($"No existe visita con id '{id}'");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Result<BookingDTO>.Failure("NO tiene permiso para visualizar esta visita");
                }

                if (booking.Status == BookingStatusEnum.Cancelled)
                {
                    return Result<BookingDTO>.Failure("No se puede completar una visita cancelada");
                }

                if (booking.Status == BookingStatusEnum.Peding)
                {
                    return Result<BookingDTO>.Failure("No se puede completar una visita que no ha sido confirmada");
                }

                if (booking.Status == BookingStatusEnum.Completed)
                {
                    return Result<BookingDTO>.Success(ToDTO(booking), "la visita ya estaba completada");
                }

                if (booking.Status != BookingStatusEnum.Confirmed)
                {
                    return Result<BookingDTO>.Failure("Solo se pueden completar visitas confirmadas");
                }

                booking.Status = BookingStatusEnum.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                _context.Bookings.Update(booking);

                await _context.SaveChangesAsync();

                return Result<BookingDTO>.Success(ToDTO(booking), "Visita confirmada con éxito");
            }
            catch (Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Result<BookingDTO>> ConfirmAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Result<BookingDTO>.Failure($"No existe visita con id '{id}'");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Result<BookingDTO>.Failure("NO tiene permiso para visualizar esta visita");
                }

                if (booking.Status == BookingStatusEnum.Cancelled)
                {
                    return Result<BookingDTO>.Failure("No se puede confirmar una visita cancelada");
                }

                if (booking.Status == BookingStatusEnum.Completed)
                {
                    return Result<BookingDTO>.Failure("No se puede completar una visita que no ha sido confirmada");
                }

                if (booking.Status == BookingStatusEnum.Confirmed)
                {
                    return Result<BookingDTO>.Success(ToDTO(booking), "la visita ya estaba confirmada");
                }

                if (booking.Status != BookingStatusEnum.Peding)
                {
                    return Result<BookingDTO>.Failure("Solo se pueden confirmar visitas pendientes");
                }

                booking.Status = BookingStatusEnum.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                _context.Bookings.Update(booking);

                await _context.SaveChangesAsync();

                return Result<BookingDTO>.Success(ToDTO(booking), "Visita confirmada con éxito");
            }
            catch (Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Result<BookingDTO>> CreateAsync(Guid userId, CreateBookingDTO dto)
        {
            try
            {
                if (userId == Guid.Empty)
                {
                    return Result<BookingDTO>.Failure("No se pudo identificar el usuario autenticado", statusCode: StatusCodes.Status401Unauthorized);
                }

                if (dto.PropertyId == Guid.Empty)
                {
                    return Result<BookingDTO>.Failure("La propiedad es requerida");
                }

                Result<BookingDTO> propertyValidation = await ValidatePropertyAsync(dto.PropertyId);

                if (propertyValidation is not null)
                {
                    return propertyValidation;
                }

                DateTime scheduleAt = dto.ScheduledAt.NormalizeToUtc();

                Result<BookingDTO>? validation = await ValidateScheduleAsync(dto.PropertyId, userId, scheduleAt, dto.DurationInMinutes);

                if (validation is not null)
                {
                    return validation;
                }

                BookingEntity booking = new BookingEntity
                {
                    Id = Guid.CreateVersion7(),
                    PropertyId = dto.PropertyId,
                    UserId = userId,
                    ScheduledAt = scheduleAt,
                    DurationInMinutes = dto.DurationInMinutes,
                    Status = BookingStatusEnum.Peding,
                    Notes = dto.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Bookings.AddAsync(booking);

                await _context.SaveChangesAsync();

                return Result<BookingDTO>.Success(ToDTO(booking), "Visita agendada con éxito");
            }
            catch (Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Result<BookingDTO>> GetOneAsync(Guid id, Guid userId, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Result<BookingDTO>.Failure($"No existe visita con id '{id}'");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Result<BookingDTO>.Failure("NO tiene permiso para visualizar esta visita");
                }

                return Result<BookingDTO>.Success(ToDTO(booking));

            }
            catch (Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        public async Task<Result<PaginationResponse<BookingDTO>>> GetPaginationAsync(BookingPaginationRequest request, Guid userId, bool isAdmin)
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

                if (request.PropertyId.HasValue)
                {
                    query = query.Where(b => b.PropertyId == request.PropertyId.Value);
                }

                if (request.From.HasValue)
                {
                    query = query.Where(b => b.ScheduledAt >= request.From.Value.NormalizeToUtc());
                }

                if (request.To.HasValue)
                {
                    query = query.Where(b => b.ScheduledAt <= request.To.Value.NormalizeToUtc());
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(b => b.Notes.ToLower().Contains(request.Filter.ToLower()));
                }

                query = query.OrderBy(b => b.ScheduledAt);

                PagedList<BookingEntity> list = await PagedList<BookingEntity>.ToPagedListAsync(query, request);
                List<BookingDTO> items = list.Select(ToDTO).ToList();

                PaginationResponse<BookingDTO> response = new PaginationResponse<BookingDTO>
                {
                    List = new PagedList<BookingDTO>(items, list.TotalCount, list.CurrentPage, list.RecordsPerPage),
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    RecordsPerPage = list.RecordsPerPage,
                    TotalCount = list.TotalCount,
                    Filter = request.Filter,
                };

                return Result<PaginationResponse<BookingDTO>>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<PaginationResponse<BookingDTO>>.Failure(ex);
            }
        }

        public async Task<Result<BookingDTO>> UpdateAsync(Guid id, Guid userId, UpdateBookingDTO dto, bool isAdmin)
        {
            try
            {
                BookingEntity? booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

                if (booking is null)
                {
                    return Result<BookingDTO>.Failure($"No existe visita con id '{id}'");
                }

                if (!isAdmin && booking.UserId != userId)
                {
                    return Result<BookingDTO>.Failure("NO tiene permiso para visualizar esta visita");
                }

                if (booking.Status is BookingStatusEnum.Cancelled or BookingStatusEnum.Completed)
                {
                    return Result<BookingDTO>.Failure("No se puede reprogramar una visita cancelada o completada");
                }

                Result<BookingDTO> propertyValidation = await ValidatePropertyAsync(booking.PropertyId);

                if (propertyValidation is not null)
                {
                    return propertyValidation;
                }

                DateTime scheduleAt = dto.ScheduledAt.NormalizeToUtc();

                Result<BookingDTO>? validation = await ValidateScheduleAsync(booking.PropertyId, booking.UserId, scheduleAt, dto.DurationInMinutes, booking.Id);

                if (validation is not null)
                {
                    return validation;
                }

                booking.ScheduledAt = scheduleAt;
                booking.DurationInMinutes = dto.DurationInMinutes;
                booking.Notes = dto.Notes;
                booking.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Result<BookingDTO>.Success(ToDTO(booking), "Visita reprogramada con éxito");

            }
            catch (Exception ex)
            {
                return Result<BookingDTO>.Failure(ex);
            }
        }

        private BookingDTO ToDTO(BookingEntity booking)
        {
            return new BookingDTO
            {
                Id = booking.Id,
                PropertyId = booking.PropertyId,
                UserId = booking.UserId,
                ScheduledAt = booking.ScheduledAt,
                DurationInMinutes = booking.DurationInMinutes,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt,
                CancelledAt = booking.UpdatedAt,
                CancelReason = booking.CancelReason
            };
        }

        private async Task<Result<BookingDTO>?> ValidatePropertyAsync(Guid propertyId)
        {
            PropertyValidationResult validation = await _propertiesClient.ValidateAsync(propertyId);

            return validation.Status switch
            {
                PropertyValidationStatus.Available => null,
                PropertyValidationStatus.NotFound => Result<BookingDTO>.Failure("La propiedad no existe"),
                PropertyValidationStatus.Unavailable => Result<BookingDTO>.Failure("La propiedad no está disponible para visitas"),
                PropertyValidationStatus.Unauthorized => Result<BookingDTO>.Failure("No tiene acceso", statusCode: StatusCodes.Status401Unauthorized),

                _ => Result<BookingDTO>.Failure("No se pudo validar la propiedad. Intente nuevamente", statusCode: StatusCodes.Status503ServiceUnavailable)
            };
        }

        private async Task<Result<BookingDTO>?> ValidateScheduleAsync(Guid propertyId, 
                                                                      Guid userId, 
                                                                      DateTime scheduleAt, 
                                                                      int durationInMinutes,
                                                                      Guid? excludeBookingId = null)
        {
            if (scheduleAt <= DateTime.UtcNow.AddMinutes(MinimumMinutesBeforeVisit))
            {
                return Result<BookingDTO>.Failure($"La visita debe agendarse con al menos {MinimumMinutesBeforeVisit} minutos de anticipación");
            }

            if (durationInMinutes is < 15 or > 90)
            {
                return Result<BookingDTO>.Failure("La visita debe dirar entre 15 y 90 minutos");
            }

            bool propertyHasOverlap = await HasPropertyOverlapAsync(propertyId, scheduleAt, durationInMinutes, excludeBookingId);

            if (propertyHasOverlap)
            {
                return Result<BookingDTO>.Failure("La propiedad ya tiene una visita agendada en ese horario");
            }

            bool userHasOverlap = await HasUserOverlapAsync(userId, scheduleAt, durationInMinutes, excludeBookingId);

            if (userHasOverlap)
            {
                return Result<BookingDTO>.Failure("Ya tiene una visita agendada en ese horario");
            }

            return null;
        }

        private async Task<bool> HasPropertyOverlapAsync(Guid propertyId, DateTime scheduleAt, int durationInMinutes, Guid? excludeBookingId)
        {
            DateTime end = scheduleAt.AddMinutes(durationInMinutes);

            IQueryable<BookingEntity> query = _context.Bookings.Where(b => b.PropertyId == propertyId
                                                                        && (b.Status == BookingStatusEnum.Peding || b.Status == BookingStatusEnum.Confirmed)
                                                                        && b.ScheduledAt < end
                                                                        && b.ScheduledAt.AddMinutes(b.DurationInMinutes) > scheduleAt);

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }

        private async Task<bool> HasUserOverlapAsync(Guid userId, DateTime scheduleAt, int durationInMinutes, Guid? excludeBookingId)
        {
            DateTime end = scheduleAt.AddMinutes(durationInMinutes);

            IQueryable<BookingEntity> query = _context.Bookings.Where(b => b.UserId == userId
                                                                        && (b.Status == BookingStatusEnum.Peding || b.Status == BookingStatusEnum.Confirmed)
                                                                        && b.ScheduledAt < end
                                                                        && b.ScheduledAt.AddMinutes(b.DurationInMinutes) > scheduleAt);

            if (excludeBookingId.HasValue)
            {
                query = query.Where(b => b.Id != excludeBookingId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
