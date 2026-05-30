namespace PodcastCatalog.Domain.Entities
{
    public class Contributor
    {
        public Guid Id { get; private set; }

        public string DisplayName { get; private set; } = null!;
        public string? FullName { get; private set; }
        public string? Title { get; private set; }
        public string? Biography { get; private set; }
        public string? Expertise { get; private set; }
        public string? ProfileUrl { get; private set; }


        private Contributor() { } // EF Core
        Contributor(
            Guid id,
            string displayName,
            string? fullName,
            string? title,
            string? biography,
            string? expertise,
            string? profileUrl)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name required");

            Id = id;
            DisplayName = displayName;
            FullName = fullName;
            Title = title;
            Biography = biography;
            Expertise = expertise;
            ProfileUrl = profileUrl;
        }

        public static Contributor Create(
            string displayName,
            string? fullName,
            string? title,
            string? biography,
            string? expertise,
            string? profileUrl)
        {
            return new Contributor(
                Guid.NewGuid(),
                displayName,
                fullName,
                title,
                biography,
                expertise,
                profileUrl);
        }

        public void Update(
            string displayName,
            string? fullName,
            string? title,
            string? biography,
            string? expertise,
            string? profileUrl)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name required");

            DisplayName = displayName;
            FullName = fullName;
            Title = title;
            Biography = biography;
            Expertise = expertise;
            ProfileUrl = profileUrl;
        }
    }
}