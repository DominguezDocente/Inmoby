using Booking.Api.DTOs.External.Properties;
using Booking.Api.Services.External.Abtractions;

namespace Booking.Api.Services.External.Implemetations
{
    public class PropertiesClient : IPropertiesClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertiesClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PropertyValidationResult> ValidateAsync(Guid propertyId, CancellationToken cancellationToken = default)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"api/Properties/{propertyId}");

            string? authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                request.Headers.TryAddWithoutValidation("Authorization", authorization);
            }

            try
            {
                using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new PropertyValidationResult
                    {
                        Status = PropertyValidationStatus.NotFound 
                    };
                }

                if (response.StatusCode is System.Net.HttpStatusCode.Unauthorized or System.Net.HttpStatusCode.Forbidden)
                {
                    return new PropertyValidationResult
                    {
                        Status = PropertyValidationStatus.Unauthorized
                    };
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new PropertyValidationResult
                    {
                        Status = PropertyValidationStatus.ServiceUnavailable
                    };
                }

                PropertyApiResponse? payload = await response.Content.ReadFromJsonAsync<PropertyApiResponse>(cancellationToken: cancellationToken);

                if (payload is null || !payload.IsSuccess || payload.Data is null)
                {
                    return new PropertyValidationResult
                    {
                        Status = PropertyValidationStatus.NotFound
                    };
                }

                PropertyValidationStatus status = payload.Data.IsAvailable ? PropertyValidationStatus.Available
                                                                           : PropertyValidationStatus.Unavailable;

                return new PropertyValidationResult
                {
                    Status = status,
                    OwnerId = payload.Data.OwnerId
                };

            }
            catch(HttpRequestException ex)
            {
                return new PropertyValidationResult
                {
                    Status = PropertyValidationStatus.ServiceUnavailable
                };
            }
            catch (TaskCanceledException ex)
            {
                return new PropertyValidationResult
                {
                    Status = PropertyValidationStatus.ServiceUnavailable
                };
            }
            catch (Exception ex)
            {
                return new PropertyValidationResult
                {
                    Status = PropertyValidationStatus.ServiceUnavailable
                };
            }
        }
    }
}
