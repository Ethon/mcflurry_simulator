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
    public class VenueServiceTests {
        private static IDatabase db;
        private static IVenueDao vdao;
        private static CategoryDao catdao;
        private static CountryDao coudao;
        private static ArtistDao adao;
        private static PerformanceDao pdao;
        private static IVenueService vs;

        private Performance createTestPerformanceAtVenue(Venue venue) {
            Category categoryData = RepresentativeData.GetDefaultCategories()[0];
            Country countryData = RepresentativeData.GetDefaultCountries()[0];
            Artist artistData = RepresentativeData.GetDefaultArtists()[0];
            Performance performanceData = RepresentativeData.GetDefaultPerformances()[0];

            Category category = catdao.CreateCategory(categoryData.Shortcut, categoryData.Name);
            Country country = coudao.CreateCountry(countryData.Name, countryData.FlagPath);
            Artist artist = adao.CreateArtist(artistData.Name, artistData.Email, category.Id, country.Id,
                artistData.PicturePath, artistData.VideoPath);
            Performance performance = pdao.CreatePerformance(performanceData.Date, artist.Id, venue.Id);

            return performance;
        }

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext) {
            db = new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;");
            vdao = new VenueDao(db);
            catdao = new CategoryDao(db);
            coudao = new CountryDao(db);
            adao = new ArtistDao(db);
            pdao = new PerformanceDao(db);
            vs = ServiceFactory.CreateVenueService(db);
        }

        [ClassCleanup()]
        public static void ClassCleanup() {
            pdao.DeleteAllPerformances();
            vdao.DeleteAllVenues();
            adao.DeleteAllArtists();
            catdao.DeleteAllCategories();
            coudao.DeleteAllCountries();
            db.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateVenueWithInvalidShortcut() {
            vs.CreateVenue("Dreifaltigkeitssäule", "123", 48.30584975, 14.28644065);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateVenueWithInvalidLatitude() {
            vs.CreateVenue("Dreifaltigkeitssäule", "H1", 1000.0, 14.28644065);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateVenueWithInvalidLongitude() {
            vs.CreateVenue("Dreifaltigkeitssäule", "H1", 48.30584975, 1000);
        }

        [TestMethod()]
        public void CreateVenueWithValidInfo() {
            vs.CreateVenue("Dreifaltigkeitssäule", "H1", 48.30584975, 14.28644065);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateVenueWithInvalidShortcut() {
            vs.UpdateVenue(new Venue(0, "Dreifaltigkeitssäule", "123", 48.30584975, 14.28644065));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateVenueWithInvalidLatitude() {
            vs.UpdateVenue(new Venue(0, "Dreifaltigkeitssäule", "H1", 1000.0, 14.28644065));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateVenueWithInvalidLongitude() {
            vs.UpdateVenue(new Venue(0, "Dreifaltigkeitssäule", "H1", 48.30584975, 1000));
        }

        [TestMethod()]
        public void UpdateVenueWithValidInfo() {
            Venue venue = vs.CreateVenue("Dreifaltigkeitssäule", "H1", 48.30584975, 14.28644065);
            venue.Name = "Other name";
            venue.Shortcut = "O1";
            venue.Longitude = 55.67;
            venue.Latitude = 22.990;
            vs.UpdateVenue(venue);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void DeleteUsedVenue() {
            Venue venue = vs.CreateVenue("Dreifaltigkeitssäule", "H1", 48.30584975, 14.28644065);
            createTestPerformanceAtVenue(venue);
            vs.DeleteVenue(venue);
        }

        [TestMethod()]
        public void DeleteUnusedVenue() {
            Venue venue = vs.CreateVenue("Dreifaltigkeitssäule", "H1", 48.30584975, 14.28644065);
            vs.DeleteVenue(venue);
        }
    }
}