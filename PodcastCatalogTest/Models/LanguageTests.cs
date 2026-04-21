//using PodcastCatalog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PodcastCatalogTest.Models
//{
//    [TestClass]
//    public class LanguageTests
//    {
//        [TestMethod]
//        public void CreateLanguage_WithValidData_SetsProperties()
//        {
//            // Arrange
//            var id = 1;
//            var name = "Danish";
//            var iso = "da";

//            // Act
//            var language = Language.Create(id, name, iso);

//            // Assert
//            Assert.AreEqual(id, language.Id);
//            Assert.AreEqual(name, language.Name);
//            Assert.AreEqual(iso, language.ISO);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void CreateLanguage_WithEmptyName_ThrowsException()
//        {
//            // Arrange
//            var id = 1;
//            var name = "";
//            var iso = "da";

//            // Act
//            var language = Language.Create(id, name, iso);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void CreateLanguage_WithEmptyIso_ThrowsException()
//        {
//            // Arrange
//            var id = 1;
//            var name = "Danish";
//            var iso = "";

//            // Act
//            var language = Language.Create(id, name, iso);
//        }

//        [TestMethod]
//        public void UpdateLanguage_WithValidData_UpdatesProperties()
//        {
//            // Arrange
//            var language = Language.Create(1, "Danish", "da");
//            var newName = "English";
//            var newIso = "en";

//            // Act
//            language.Update(newName, newIso);

//            // Assert
//            Assert.AreEqual(newName, language.Name);
//            Assert.AreEqual(newIso, language.ISO);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void UpdateLanguage_WithEmptyName_ThrowsException()
//        {
//            // Arrange
//            var language = Language.Create(1, "Danish", "da");
//            var newName = "";
//            var newIso = "en";

//            // Act
//            language.Update(newName, newIso);
//        }
//    }
//}
