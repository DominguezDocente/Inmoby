namespace Booking.Api.Services.External.Abtractions
{
    public enum PropertyValidationStatus
    {
        Available = 1,
        NotFound = 2,
        Unavailable = 3,
        Unauthorized = 4,
        ServiceUnavailable = 5
    }
}
