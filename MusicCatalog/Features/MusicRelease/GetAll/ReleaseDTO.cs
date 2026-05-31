using MusicCatalogContext.Domain.Enums;

namespace MusicCatalogContext.Features.MusicRelease.GetAll
{
    public class ReleaseDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? EAN { get; set; }
        public DateTime? ReleaseDate { get; set; }


        public string? GenreName { get; set; }

        public string? LanguageName { get; set; }
        public ReleaseStatus? Status { get; set; }
    }
}
