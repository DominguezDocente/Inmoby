namespace Properties.Application.UseCases.Properties.Commands.CreateProperty
{
    public sealed class PropertyImageInput
    {
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
