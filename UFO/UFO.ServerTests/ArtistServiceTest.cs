using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;
using System.Diagnostics;

namespace UFO.Server.Tests {
    [TestClass()]
    public class ArtistServiceTests {
        private static IDatabase db;
        private static ICategoryDao catdao;
        private static ICountryDao coudao;
        private static IArtistDao adao;
        private static IVenueDao vdao;
        private static IPerformanceDao pdao;

        private static IArtistService aS;

        private Country country;
        private Category category;


        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext) {
            db = new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;");

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

        [TestInitialize()]
        public void Startup() {
            catdao = new CategoryDao(db);
            coudao = new CountryDao(db);
            adao = new ArtistDao(db);
            vdao = new VenueDao(db);
            pdao = new PerformanceDao(db);
            aS = ServiceFactory.CreateArtistService(db);
            category = RepresentativeData.GetDefaultCategories()[0];
            country = RepresentativeData.GetDefaultCountries()[0];
            category = catdao.CreateCategory(category.Shortcut, category.Name);
            country = coudao.CreateCountry(country.Name, country.FlagPath);
        }

        [TestCleanup()]
        public void Cleanup() {
            pdao.DeleteAllPerformances();
            vdao.DeleteAllVenues();
            adao.DeleteAllArtists();
            catdao.DeleteAllCategories();
            coudao.DeleteAllCountries();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateArtistWithInvalidName() {
            aS.CreateArtist("123Name", "max@muster.de", category, country, "max.png", "max.mp4");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateArtistWithInvalidEMail() {
            aS.CreateArtist("Max Muster", "max!muster.de", category, country, "max.png", "max.mp4");
        }

        [TestMethod()]
        public void CreateArtistWithValidInfo() {
            aS.CreateArtist("Peter Mueller", "peter@mueller.com", category, country, "mueller.png", "mueller.mp4");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateArtistWithInvalidName() {
            aS.UpdateArtist(new Artist(0,"123Name", "max@muster.de", category.Id, country.Id, "max.png", "max.mp4"));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateArtistWithInvalidEMail() {
            aS.UpdateArtist(new Artist(0, "Max Muster", "max!muster.de", category.Id, country.Id, "max.png", "max.mp4"));
        }

        [TestMethod()]
        public void UpdateArtistWithValidInfo() {

            Artist artist = aS.CreateArtist("Max Muster", "max@muster.de", category, country, "max.png", "max.mp4");
            artist.Name = "Hermann Maier";
            artist.Email = "hermann@maier.de";
            
            coudao.UpdateCountry(country);
        }

        [TestMethod()]
        public void DeleteArtistWithPastPerformances() {

            Venue venueData = RepresentativeData.GetDefaultVenues()[0];
            Venue venue = vdao.CreateVenue(venueData.Name, venueData.Shortcut, venueData.Latitude, venueData.Longitude);
            Artist artist = aS.CreateArtist("Max Muster", "max@muster.de", category, country, "max.png", "max.mp4");

            Performance pPast1 = pdao.CreatePerformance(new DateTime(2015, 1, 1, 12, 0, 0), artist.Id, venue.Id);
            Performance pPast2 = pdao.CreatePerformance(new DateTime(2015, 1, 1, 17, 0, 0), artist.Id, venue.Id);

            Performance pFuture1 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 12, 0, 0), artist.Id, venue.Id);
            Performance pFuture2 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 17, 0, 0), artist.Id, venue.Id);
            Performance pFuture3 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 21, 0, 0), artist.Id, venue.Id);

            Assert.AreEqual((uint)5, pdao.CountOfPerformancesOfArtist(artist));

            aS.DeleteArtist(artist);
            var allArtists = aS.GetAllArtists();
            Assert.AreEqual(0, allArtists.Count);
            artist = aS.GetArtistById(artist.Id);

            Assert.AreEqual((uint)2, pdao.CountOfPerformancesOfArtist(artist));
        }

        [TestMethod()]
        public void DeleteArtistWithFuturePerformances() {
            Venue venueData = RepresentativeData.GetDefaultVenues()[0];
            Venue venue = vdao.CreateVenue(venueData.Name, venueData.Shortcut, venueData.Latitude, venueData.Longitude);
            Artist artist = aS.CreateArtist("Max Muster", "max@muster.de", category, country, "max.png", "max.mp4");
            Performance pFuture1 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 12, 0, 0), artist.Id, venue.Id);
            Performance pFuture2 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 17, 0, 0), artist.Id, venue.Id);
            Performance pFuture3 = pdao.CreatePerformance(new DateTime(2120, 1, 1, 21, 0, 0), artist.Id, venue.Id);

            aS.DeleteArtist(artist);
            Assert.AreEqual(0, aS.GetAllArtists().Count);
            Assert.AreEqual((uint)0, pdao.CountOfPerformancesOfArtist(artist));
        }
    }
}