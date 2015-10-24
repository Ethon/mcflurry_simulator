using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class CategoryDaoTests {

        private static IDatabase db;
        private ICategoryDao catDao;

        private List<Category> GetTestCategoryData() {
            return new List<Category>() {
            new Category(0, "A", "Akrobatik"),
            new Category(1, "C", "Comedy & Clownerie"),
            new Category(2, "F", "Feuershow"),
            new Category(3, "L", "Luftakrobatik"),
            new Category(4, "M", "Musik"),
            new Category(5, "J", "Jonglage"),
            new Category(6, "OT", "Figuren - und Objekttheater"),
            new Category(7, "S", "Samba"),
            new Category(8, "ST", "Stehstill - Statue")
        };
        }

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext) {
            db = new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;");
        }

        [ClassCleanup()]
        public static void ClassCleanup() {
            db.Dispose();
        }

        [TestInitialize()]
        public void Startup() {
            catDao = new CategoryDao(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            catDao.DeleteAllCategories();
        }

        [TestMethod()]
        public void CreateCategoryTest() {
            var testCategories = GetTestCategoryData();
            foreach (var category in testCategories) {
                catDao.CreateCategory(category.Shortcut, category.Name);
            }
            var allCategories = catDao.GetAllCategories();
            Assert.AreEqual(testCategories.Count, allCategories.Count);
        }

        [TestMethod()]
        public void DeleteCategoryTest() {
            var testCategories = GetTestCategoryData();
            
            Category c1 = catDao.CreateCategory(testCategories[0].Shortcut, testCategories[0].Name);
            Category c2 = catDao.CreateCategory(testCategories[1].Shortcut, testCategories[1].Name);
            var allusers = udao.GetAllUsers();
            Assert.AreEqual(2, allusers.Count);
            udao.DeleteUser(u1);
            allusers = udao.GetAllUsers();
            Assert.AreEqual(1, allusers.Count);
            Assert.IsNull(udao.GetUserById(u1.Id));
            Assert.IsNotNull(udao.GetUserById(u2.Id));
        }

        [TestMethod()]
        public void GetAllCategoriesTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCategoryByIdTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateCategoryTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAllCategoriesTest() {
            Assert.Fail();
        }
    }
}