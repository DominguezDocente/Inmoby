namespace Booking.Api.Services.External.Abtractions
{
    public record PropertyValidationResult
    {
        public PropertyValidationStatus Status { get; set; }
        public Guid? OwnerId { get; set; } = null;
    }
}
