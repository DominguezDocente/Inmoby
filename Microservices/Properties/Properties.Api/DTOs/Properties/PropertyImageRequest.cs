using System.ComponentModel.DataAnnotations;

namespace Properties.Api.DTOs.Properties
{
    public sealed class PropertyImageRequest
    {
        [Required]
        [StringLength(1024)]
        public string Url { get; set; } = string.Empty;

        [StringLength(1024)]
        public string? Description { get; set; }

        public bool IsPrimary { get; set; }

        [Range(0, int.MaxValue)]
        public int DisplayOrder { get; set; }
    }
}
