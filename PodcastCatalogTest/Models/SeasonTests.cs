//using FluentAssertions;
//using PodcastCatalog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PodcastCatalogTest.Models
//{
//    [TestClass]
//    public class SeasonTests
//    {
//        [TestMethod]
//        public void Constructor_ShouldSetProperties()
//        {
//            var season = CreateSeason();

//            season.Name.Should().Be("Season 1");
//        }

//        [TestMethod]
//        public void Constructor_ShouldThrow_WhenNameIsEmpty()
//        {
//            Action act = () => new Season(Guid.NewGuid(), "", DateOnly.FromDateTime(DateTime.Today));

//            act.Should().Throw<ArgumentException>();
//        }

//        [TestMethod]
//        public void CreateEpisode_ShouldAddEpisode()
//        {
//            var season = CreateSeason();

//            var episode = season.CreateEpisode(
//                "Ep1",
//                "Desc",
//                TimeSpan.FromMinutes(10),
//                DateOnly.FromDateTime(DateTime.Today));

//            season.Episodes.Should().Contain(episode);
//        }

//        [TestMethod]
//        public void AddEpisode_ShouldAddEpisode()
//        {
//            var season = CreateSeason();
//            var episode = CreateEpisode();

//            season.AddEpisode(episode);

//            season.Episodes.Should().ContainSingle();
//        }

//        [TestMethod]
//        public void AddEpisode_ShouldNotAddDuplicate()
//        {
//            var season = CreateSeason();
//            var episode = CreateEpisode();

//            season.AddEpisode(episode);
//            season.AddEpisode(episode);

//            season.Episodes.Count.Should().Be(1);
//        }

//        [TestMethod]
//        public void RemoveEpisode_ShouldRemoveEpisode()
//        {
//            var season = CreateSeason();
//            var episode = CreateEpisode();

//            season.AddEpisode(episode);

//            season.RemoveEpisode(episode.Id);

//            season.Episodes.Should().BeEmpty();
//        }

//        private Season CreateSeason()
//        {
//            return new Season(
//                Guid.NewGuid(),
//                "Season 1",
//                DateOnly.FromDateTime(DateTime.Today));
//        }

//        private Episode CreateEpisode()
//        {
//            return new Episode(
//                Guid.NewGuid(),
//                "Test",
//                "Desc",
//                TimeSpan.FromMinutes(10),
//                DateOnly.FromDateTime(DateTime.Today));
//        }

//    }
//}
