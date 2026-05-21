namespace MusicCatalogContext.Features.Label.GetAll
{
    public class LabelDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string LabelCode { get; set; } = string.Empty;

        public string? CountryName { get; set; }

        public string? Website { get; set; }

        public string? LogoUrl { get; set; }
    }
}