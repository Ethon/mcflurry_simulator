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
            return RepresentativeData.GetDefaultCategories();
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
            var allCategories = catDao.GetAllCategories();
            Assert.AreEqual(2,2);
            catDao.DeleteCategory(c1);
            allCategories = catDao.GetAllCategories();
            Assert.AreEqual(1, allCategories.Count);
            Assert.IsNull(catDao.GetCategoryById(c1.Id));
            Assert.IsNotNull(catDao.GetCategoryById(c2.Id));
        }

        [TestMethod()]
        public void GetAllCategoriesTest() {
            var testCategories = GetTestCategoryData();
            for (int i = 0; i < testCategories.Count; ++i) {
                testCategories[i] = catDao.CreateCategory(testCategories[i].Shortcut,testCategories[i].Name);
                Assert.IsNotNull(testCategories[i]);
            }
            var allCategories = catDao.GetAllCategories();
            Assert.AreEqual(testCategories.Count, allCategories.Count);
            for (int i = 0; i < allCategories.Count; ++i) {
                Assert.AreEqual(allCategories[i], testCategories[i]);
            }
        }

        [TestMethod()]
        public void GetCategoryByIdTest() {
            var testCategories = GetTestCategoryData();
            Category c1 = catDao.CreateCategory(testCategories[0].Shortcut, testCategories[0].Name);
            Category c2 = catDao.CreateCategory(testCategories[1].Shortcut, testCategories[1].Name);
            Category c3 = catDao.GetCategoryById(c1.Id);
            Assert.IsNotNull(c3);
            Assert.AreEqual(c1, c3);
            Assert.AreNotEqual(c2, c3);
            catDao.DeleteCategory(c3);
            Assert.IsNull(catDao.GetCategoryById(c3.Id));
        }

        [TestMethod()]
        public void UpdateCategoryTest() {
            var testCategories = GetTestCategoryData();
            Category c1 = catDao.CreateCategory(testCategories[0].Shortcut, testCategories[0].Name);
            c1.Shortcut = "nC";
            c1.Name = "newName";
            catDao.UpdateCategory(c1);
            Category c2 = catDao.GetCategoryById(c1.Id);
            Assert.AreEqual(c1, c2);
        }

        [TestMethod()]
        public void DeleteAllCategoriesTest() {
            var testCategories = GetTestCategoryData();

            foreach (var item in testCategories) {
                catDao.CreateCategory(item.Shortcut, item.Name);
            }
            Assert.AreEqual(catDao.GetAllCategories().Count, testCategories.Count);
            catDao.DeleteAllCategories();
            Assert.AreEqual(0, catDao.GetAllCategories().Count);

        }
    }
}