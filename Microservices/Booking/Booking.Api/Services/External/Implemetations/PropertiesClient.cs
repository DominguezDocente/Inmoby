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

        public Task<PropertyValidationResult> ValidateAsync(Guid propertyId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
