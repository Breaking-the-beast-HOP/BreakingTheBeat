//using PodcastCatalog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PodcastCatalogTest.Models
//{
//    [TestClass]
//    public class CategoryTests
//    {
//        [TestMethod]
//        public void CreateCategory_WithValidData_SetsProperties()
//        {
//            // Arrange
//            var id = 1;
//            var name = "Technology";
//            var description = "All about tech";

//            // Act
//            var category = Category.Create(id, name, description);

//            // Assert
//            Assert.AreEqual(id, category.Id);
//            Assert.AreEqual(name, category.Name);
//            Assert.AreEqual(description, category.Description);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void CreateCategory_WithEmptyName_ThrowsException()
//        {
//            // Arrange
//            var id = 1;
//            var name = "";
//            var description = "All about tech";

//            // Act
//            var category = Category.Create(id, name, description);
//        }

//        [TestMethod]
//        public void UpdateCategory_WithValidData_UpdatesProperties()
//        {
//            // Arrange
//            var category = Category.Create(1, "Technology", "All about tech");
//            var newName = "Science";
//            var newDescription = "All about science";

//            // Act
//            category.Update(newName, newDescription);

//            // Assert
//            Assert.AreEqual(newName, category.Name);
//            Assert.AreEqual(newDescription, category.Description);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException))]
//        public void UpdateCategory_WithEmptyName_ThrowsException()
//        {
//            // Arrange
//            var category = Category.Create(1, "Technology", "All about tech");
//            var newName = "";
//            var newDescription = "All about science";

//            // Act
//            category.Update(newName, newDescription);
//        }
//    }
//}
