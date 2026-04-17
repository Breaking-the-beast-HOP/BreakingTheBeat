using PodcastCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodcastCatalogTest.Models
{
    [TestClass]
    public class PodcastTests
    {
        [TestMethod]
        public void CreatePodcast_WithValidData_SetsProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var title = "Test Podcast";
            var description = "Test Description";

            // Act
            var podcast = new Podcast(id, title, description);

            // Assert
            Assert.AreEqual(id, podcast.Id);
            Assert.AreEqual(title, podcast.Title);
            Assert.AreEqual(description, podcast.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePodcast_WithEmptyTitle_ThrowsException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var title = "";
            var description = "Test Description";

            // Act
            var podcast = new Podcast(id, title, description);
        }

        [TestMethod]
        public void AddSeason_WithValidSeason_AddsSeason()
        {
            // Arrange
            var podcast = new Podcast(Guid.NewGuid(), "Test Podcast", "Test Description");
            var season = new Season(Guid.NewGuid(), "Season 1", new DateOnly(2023, 1, 1));

            // Act
            podcast.AddSeason(season);

            // Assert
            Assert.AreEqual(1, podcast.Seasons.Count);
        }

        [TestMethod]
        public void AddSeason_WithDuplicateSeason_DoesNotAdd()
        {
            // Arrange
            var podcast = new Podcast(Guid.NewGuid(), "Test Podcast", "Test Description");
            var season = new Season(Guid.NewGuid(), "Season 1", new DateOnly(2023, 1, 1));
            podcast.AddSeason(season);

            // Act
            podcast.AddSeason(season);

            // Assert
            Assert.AreEqual(1, podcast.Seasons.Count);
        }

        [TestMethod]
        public void RemoveSeason_WithExistingSeason_RemovesSeason()
        {
            // Arrange
            var podcast = new Podcast(Guid.NewGuid(), "Test Podcast", "Test Description");
            var season = new Season(Guid.NewGuid(), "Season 1", new DateOnly(2023, 1, 1));
            podcast.AddSeason(season);

            // Act
            podcast.RemoveSeason(season.Id);

            // Assert
            Assert.AreEqual(0, podcast.Seasons.Count);
        }
    }
}
