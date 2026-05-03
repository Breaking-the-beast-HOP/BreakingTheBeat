
    namespace MusicCatalog.Features.Release.GetById
    {
        public class ReleaseDetailsDto
        {
            public Guid Id { get; set; }
            public string? Title { get; set; }
            public string? Version { get; set; }
            public string? EAN { get; set; }
            public DateTime? ReleaseDate { get; set; }

            public Guid? GenreId { get; set; }
            public string? GenreName { get; set; }

            public Guid? LanguageId { get; set; }
            public string? LanguageName { get; set; }

            public bool HasCover { get; set; }
            public bool HasBooklet { get; set; }

            public List<ReleaseVolumeDto> Volumes { get; set; } = new();
            public List<ContributionDto> Contributions { get; set; } = new();
        }

        public class ReleaseVolumeDto
        {
            public Guid Id { get; set; }
            public int VolumeNumber { get; set; }
            public List<ReleaseTrackDto> Tracks { get; set; } = new();
        }

        public class ReleaseTrackDto
        {
            public Guid TrackId { get; set; }
            public int TrackNumber { get; set; }

            public string? Title { get; set; }
            public string? Version { get; set; }
            public string? ISRC { get; set; }
            public int DurationSeconds { get; set; }
        }

        public class ContributionDto
        {
            public Guid ContributorId { get; set; }
            public string ContributorName { get; set; } = "";
            public List<ContributionRoleDto> Roles { get; set; } = new();
        }

        public class ContributionRoleDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = "";
        }
    }
