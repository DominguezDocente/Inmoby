namespace Booking.Api.Core.Extensions
{
    public static class DateExtensions
    {

        public static DateTime NormalizeToUtc(this DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
            };
        }
    }
}
