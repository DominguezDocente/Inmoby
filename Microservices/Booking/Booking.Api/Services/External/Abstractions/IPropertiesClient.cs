namespace Booking.Api.Services.External.Abstractions
{
    public interface IPropertiesClient
    {
        Task<PropertyValidationResult> ValidateAsync(Guid propertyId, CancellationToken cancellationToken = default);
    }
}
