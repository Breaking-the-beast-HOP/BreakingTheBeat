using System.Formats.Asn1;

namespace PodcastCatalog.Models
{
    public class Episode
    {
        private readonly List<Tag> _tags = new();
        private readonly List<Contribution> _contributions = new();

        public Guid Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateOnly AiringDate { get; private set; }

        public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();
        public IReadOnlyList<Contribution> Contributions => _contributions.AsReadOnly();

        public Episode(Guid id, string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            Id = id;
            Update(title, description, duration, airingDate);
        }

        public void Update(string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title required");

            Title = title;
            Description = description;
            Duration = duration;
            AiringDate = airingDate;
        }

        // 🔹 TAGS
        public void AddTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));

            if (_tags.Any(t => t.Id == tag.Id))
                return;

            _tags.Add(tag);
        }

        public void RemoveTag(Guid tagId)
        {
            _tags.RemoveAll(t => t.Id == tagId);
        }

        // 🔹 CONTRIBUTIONS
        public void AddContribution(Contributor contributor, Role role, bool hasRights)
        {
            if (_contributions.Any(c =>
                c.Contributor.Id == contributor.Id &&
                c.Role.Id == role.Id))
            {
                return;
            }

            _contributions.Add(new Contribution(Guid.NewGuid(), contributor, role, hasRights));
        }

        public void RemoveContribution(Guid contributorId, Guid roleId)
        {
            _contributions.RemoveAll(c =>
                c.Contributor.Id == contributorId &&
                c.Role.Id == roleId);
        }
    }
}