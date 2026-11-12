using System.ComponentModel.DataAnnotations;

namespace Booking.Api.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is Guid id && id != Guid.Empty;
        }
    }
}
