namespace Booking.Api.Services.External.Abtractions
{
    public interface IPropertiesClient
    {
        Task<PropertyValidationResult> ValidateAsync(Guid propertyId, CancellationToken cancellationToken = default);
    }
}
