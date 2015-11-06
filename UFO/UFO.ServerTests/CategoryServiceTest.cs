using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server.Tests {
    [TestClass()]
    public class CategoryServiceTests {
        private static IDatabase db;
        private static ICategoryDao catdao;
        private static ICountryDao coudao;
        private static IArtistDao adao;
        private static CategoryService catS;

        private Artist createTestArtistOfCategory(Category category) {
            Country countryData = RepresentativeData.GetDefaultCountries()[0];
            Artist artistData = RepresentativeData.GetDefaultArtists()[0];

            Country country = coudao.CreateCountry(countryData.Name, countryData.FlagPath);
            Artist artist = adao.CreateArtist(artistData.Name, artistData.Email, category.Id, country.Id,
                artistData.PicturePath, artistData.VideoPath);
            return artist;
        }

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext) {
            db = new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;");
            catdao = new CategoryDao(db);
            coudao = new CountryDao(db);
            adao = new ArtistDao(db);
            catS = new CategoryService(db);
        }

        [ClassCleanup()]
        public static void ClassCleanup() {
            adao.DeleteAllArtists();
            catdao.DeleteAllCategories();
            coudao.DeleteAllCountries();
            db.Dispose();
        }

        [TestCleanup()]
        public void Cleanup() {
            adao.DeleteAllArtists();
            catdao.DeleteAllCategories();
            coudao.DeleteAllCountries();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateCategoryWithInvalidShortcut() {
            catS.CreateCategory("ABC","ABC-CATEGORY");
        }


        [TestMethod()]
        public void CreateCategoryWithValidInfo() {
            catS.CreateCategory("tu", "Turnakrobatik");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateCategoryWithInvalidShortcut() {
            Category category = new Category(0, "TU", "TURNAKROBATIK");
            category = catS.CreateCategory(category.Shortcut, category.Name);
            category.Shortcut = "ABC";
            category.Name = "Jodeln";
            catS.UpdateCategory(category);
        }


        [TestMethod()]
        public void UpdateCategoryWithValidInfo() {
            Category category = new Category(0, "TU", "TURNAKROBATIK");
            category = catS.CreateCategory(category.Shortcut,category.Name);
            category.Shortcut = "JO";
            category.Name = "Jodeln";
            catS.UpdateCategory(category);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void DeleteUsedCategory() {
            Category category = catS.CreateCategory("TU", "Turnakrobatik");
            createTestArtistOfCategory(category);
            catS.DeleteCategory(category);
        }

        [TestMethod()]
        public void DeleteUnusedCategory() {
            Category category = catS.CreateCategory("TU", "Turnakrobatik");
            catS.DeleteCategory(category);
        }
    }
}