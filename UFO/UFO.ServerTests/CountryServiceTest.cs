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
    public class CountryServiceTests {
        private static IDatabase db;
        private static ICategoryDao catdao;
        private static ICountryDao coudao;
        private static IArtistDao adao;
        private static ICountryService couS;

        private Artist createTestArtistOfCountry(Country country) {
            Category categoryData = RepresentativeData.GetDefaultCategories()[0];
            Artist artistData = RepresentativeData.GetDefaultArtists()[0];

            Category category = catdao.CreateCategory(categoryData.Shortcut, categoryData.Name);
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
            couS = ServiceFactory.CreateCountryService(db);
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
        public void CreateCountryWithInvalidInfo() {
            couS.CreateCountry("123Land", "pic.png");
        }


        [TestMethod()]
        public void CreateCountryWithValidInfo() {
            couS.CreateCountry("Deutschland", "de.png");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateCountryWithInvalidInfo() {
            Country country = new Country(0, "Deutschland", "de.png");
            country = couS.CreateCountry(country.Name, country.FlagPath);
            country.Name = "123Land";
            couS.UpdateCountry(country);
        }


        [TestMethod()]
        public void UpdateCountryWithValidInfo() {
            Country country = new Country(0, "Deutschland", "de.png");
            country = couS.CreateCountry(country.Name, country.FlagPath);
            country.Name = "Frankreich";
            country.FlagPath = "fr.png";
            coudao.UpdateCountry(country);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void DeleteUsedCountry() {
            Country country = couS.CreateCountry("Deutschland", "de.png");
            createTestArtistOfCountry(country);
            couS.DeleteCountry(country);
        }

        [TestMethod()]
        public void DeleteUnusedCountry() {
            Country country = couS.CreateCountry("Deutschland", "de.png");
            couS.DeleteCountry(country);
        }
    }
}