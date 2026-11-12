namespace Booking.Api.DTOs.External
{
    public sealed class PropertyApiResponse
    {
        public bool IsSuccess { get; set; }
        public PropertyData? Data { get; set; }
    }

    public sealed class PropertyData
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
