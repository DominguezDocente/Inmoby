namespace Booking.Api.DTOs.External.Properties
{
    public class PropertyApiResponse
    {
        public bool IsSuccess { get; set; }
        public PropertyData? Data { get; set; }
    }

    public class PropertyData
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
