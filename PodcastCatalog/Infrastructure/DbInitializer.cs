using PodcastCatalog.Domain.Entities;
using PodcastCatalog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalog.Infrastructure
{
    public static class DbInitializer
    {
        public static void Initialize(PodcastCatalogDbContext context)
        {
            context.Database.EnsureCreated();

            // Opret dummydata
            var languages = new Language[]
            {
                new Language(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Placeholder", "ph"),
                new Language("Danish", "da"),
                new Language("English", "en"),
                new Language("German", "de")
            };

            if (!context.Languages.Any())
            {
                context.Languages.AddRange(languages);
                context.SaveChanges();
            }   

            var categories = new Category[]
            {
                new Category(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Placeholder", "Placeholder"),
                new Category("Technology", "Podcasts about technology"),
                new Category("Education", "Podcasts about education"),
                new Category("Entertainment", "Podcasts for entertainment")
            };

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }    

            var podcasts = new Podcast[]
            {
                new Podcast("Tech Talks", "A podcast about the latest in tech"),
                new Podcast("Learn with Us", "Educational content for everyone")
            };
            podcasts[0].AddCategory(categories[0]);
            podcasts[0].AddLanguage(languages[0]);
            podcasts[1].AddCategory(categories[1]);
            podcasts[1].AddLanguage(languages[1]);

            if (!context.Podcasts.Any())
            {
                context.Podcasts.AddRange(podcasts);
                context.SaveChanges();
            }

            var seasons = new Season[]
            {
                new Season("Season 1", new DateOnly(2023, 1, 1), podcasts[0].Id, podcasts[0]),
                new Season("Season 2", new DateOnly(2023, 6, 1), podcasts[0].Id, podcasts[0]),
                new Season("Season 1", new DateOnly(2023, 1, 1), podcasts[1].Id, podcasts[1])
            };

            if (!context.Seasons.Any())
            {
                context.Seasons.AddRange(seasons);
                context.SaveChanges();
            }

            var tags = new Tag[]
            {
                new Tag(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Placeholder"),
                new Tag("Technology"),
                new Tag("Education"),
                new Tag("Programming"),
                new Tag("Science")
            };

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(tags);
                context.SaveChanges();
            }

            var episodes = new Episode[]
            {
                new Episode("Introduction to AI", "Learn about the basics of AI", TimeSpan.FromMinutes(45), new DateOnly(2023, 1, 15), seasons[0])
                {
                },
                new Episode("Advanced Machine Learning", "Dive deep into machine learning", TimeSpan.FromMinutes(60), new DateOnly(2023, 2, 1), seasons[0])
                {
                },
                new Episode("Getting Started with C#", "Learn the basics of C#", TimeSpan.FromMinutes(30), new DateOnly(2023, 1, 20), seasons[2])
                {
                }
            };
            episodes[0].SetTag(tags[0]);
            episodes[1].SetTag(tags[0]);
            episodes[2].SetTag(tags[2]);

            if (!context.Episodes.Any())
            {
                context.Episodes.AddRange(episodes);
                context.SaveChanges();
            }

            var contributorRoles = new ContributorRole[]
            {
                new ContributorRole(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Placeholder", "Placeholder"),
                new ContributorRole("Host", "The host of the podcast"),
                new ContributorRole("Guest", "A guest on the podcast"),
                new ContributorRole("Producer", "The producer of the podcast")
            };
            if (!context.ContributorRoles.Any())
            {
                context.ContributorRoles.AddRange(contributorRoles);
                context.SaveChanges();
            }

            var contributors = new Contributor[]
            {
                new Contributor(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Placeholder", "Placeholder"),
                new Contributor("John Doe", "Tech expert and host"),
                new Contributor("Jane Smith", "Educator and guest"),
                new Contributor("Bob Johnson", "Producer")
            };
            if (!context.Contributors.Any())
            {
                context.Contributors.AddRange(contributors);
                context.SaveChanges();
            }

            var contributions = new Contribution[]
            {
                new Contribution(contributors[0], contributorRoles[0], episodes[0]),
                new Contribution(contributors[1], contributorRoles[1], episodes[0]),
                new Contribution(contributors[2], contributorRoles[2], episodes[0])
            };
            if (!context.Contributors.Any())
            {
                context.Contributions.AddRange(contributions);
                context.SaveChanges();
            }
        }
    }
}