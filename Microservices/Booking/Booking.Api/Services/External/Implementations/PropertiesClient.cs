using Booking.Api.DTOs.External;
using Booking.Api.Services.External.Abstractions;
using System.Net;

namespace Booking.Api.Services.External.Implementations
{
    public sealed class PropertiesClient : IPropertiesClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertiesClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PropertyValidationResult> ValidateAsync(Guid propertyId,
                                                                  CancellationToken cancellationToken = default)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, $"api/Properties/{propertyId}");

            string? authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrWhiteSpace(authorization))
            {
                request.Headers.TryAddWithoutValidation("Authorization", authorization);
            }

            try
            {
                using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new PropertyValidationResult(PropertyValidationStatus.NotFound);
                }

                if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
                {
                    return new PropertyValidationResult(PropertyValidationStatus.Unauthorized);
                }

                if (!response.IsSuccessStatusCode)
                {
                    return new PropertyValidationResult(PropertyValidationStatus.ServiceUnavailable);
                }

                PropertyApiResponse? payload = await response.Content.ReadFromJsonAsync<PropertyApiResponse>(cancellationToken: cancellationToken);

                if (payload is null || !payload.IsSuccess || payload.Data is null)
                {
                    return new PropertyValidationResult(PropertyValidationStatus.NotFound);
                }

                PropertyValidationStatus status = payload.Data.IsAvailable
                    ? PropertyValidationStatus.Available
                    : PropertyValidationStatus.Unavailable;

                return new PropertyValidationResult(status, payload.Data.OwnerId);
            }
            catch (HttpRequestException)
            {
                return new PropertyValidationResult(PropertyValidationStatus.ServiceUnavailable);
            }
            catch (TaskCanceledException)
            {
                return new PropertyValidationResult(PropertyValidationStatus.ServiceUnavailable);
            }
        }
    }
}
