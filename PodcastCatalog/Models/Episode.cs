using System.Formats.Asn1;

namespace PodcastCatalog.Models
{
    public class Episode
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private DateOnly _airingDate;

        public DateOnly AiringDate
        {
            get { return _airingDate; }
            set { _airingDate = value; }
        }

        private TimeSpan _duration;

        public TimeSpan Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }


        private readonly List<Tag> _tags;

        public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();

        private readonly List<Contribution> _guests;

        public IReadOnlyList<Contribution> Guests => _guests.AsReadOnly();

        public Episode UpdateEpisode(Episode entity, string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            entity.Title = title;
            entity.Description = description;
            entity.Duration = duration;
            entity.AiringDate = airingDate;
            return entity;
        }
        public Episode CreateEpisode(int id, string title, string description, TimeSpan duration, DateOnly airingDate)
        {
            return new Episode
            {
                Id = id,
                Title = title,
                Description = description,
                Duration = duration,
                AiringDate = airingDate
            };
        }

        public Episode CreateEpisode(Episode entity)
        {
            return new Episode
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Duration = entity.Duration,
                AiringDate = entity.AiringDate
            };
        }

        public void AddTag(Tag tag)
        {
            _tags.Add(tag);
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                _tags.Add(tag);
            }
        }

        public void DeleteTag(Tag tag)
        {
            _tags.Remove(tag);
        }
    }
}