using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.ServerTests {
    [TestClass()]
    public class PerformanceServiceTests {
        private static IDatabase db;
        private static IPerformanceDao pdao;
        private static IArtistDao adoa;
        private static ICountryDao countrydao;
        private static ICategoryDao categorydao;
        private static IVenueDao vdao;
        private static PerformanceService ps;

        private Artist createTestArtist(int index = 0) {
            Country co = RepresentativeData.GetDefaultCountries()[index];
            co = countrydao.CreateCountry(co.Name, co.FlagPath);

            Category ca = RepresentativeData.GetDefaultCategories()[index];
            ca = categorydao.CreateCategory(ca.Shortcut, ca.Name);

            Artist a = RepresentativeData.GetDefaultArtists()[index];
            return adoa.CreateArtist(a.Name, a.Email, ca.Id, co.Id, a.PicturePath, a.VideoPath);
        }

        private Venue createTestVenue(int index = 0) {
            Venue v = RepresentativeData.GetDefaultVenues()[index];
            return vdao.CreateVenue(v.Name, v.Shortcut, v.Latitude, v.Longitude);
        }

        private Performance CreateTestPerformance(int index = 0) {
            DateTime now = DateTime.Now;
            return ps.CreatePerformance(new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0),
                createTestArtist(index), createTestVenue(index));
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
            pdao = new PerformanceDao(db);
            adoa = new ArtistDao(db);
            countrydao = new CountryDao(db);
            categorydao = new CategoryDao(db);
            vdao = new VenueDao(db);
            ps = new PerformanceService(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            pdao.DeleteAllPerformances();
            adoa.DeleteAllArtists();
            countrydao.DeleteAllCountries();
            categorydao.DeleteAllCategories();
            vdao.DeleteAllVenues();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreatePerformanceWithDateInThePast() {
            ps.CreatePerformance(new DateTime(2014, 1, 1), createTestArtist(), createTestVenue());
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreatePerformanceWithDateFarInTheFuture() {
            ps.CreatePerformance(new DateTime(2300, 1, 1), createTestArtist(), createTestVenue());
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreatePerformanceWithInvalidTime() {
            DateTime now = DateTime.Now;
            ps.CreatePerformance(new DateTime(now.Year, now.Month, now.Day, now.Hour, 13, 0),
                createTestArtist(), createTestVenue());
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreatePerformanceWithTakenVenue() {
            Performance p = CreateTestPerformance();
            ps.CreatePerformance(p.Date, createTestArtist(1), vdao.GetVenueById(p.VenueId));
        }

        [TestMethod()]
        public void CreatePerformanceWithValidInfo() {
            CreateTestPerformance();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdatePerformanceWithDateInThePast() {
            Performance p = CreateTestPerformance();
            p.Date = new DateTime(2014, 1, 1);
            ps.UpdatePerformance(p);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdatePerformanceWithDateFarInTheFuture() {
            Performance p = CreateTestPerformance();
            p.Date = new DateTime(2300, 1, 1);
            ps.UpdatePerformance(p);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdatePerformanceWithInvalidTime() {
            Performance p = CreateTestPerformance();
            DateTime now = DateTime.Now;
            p.Date = new DateTime(now.Year, now.Month, now.Day, now.Hour, 13, 0);
            ps.UpdatePerformance(p);
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdatePerformanceWithTakenVenue() {
            Performance p1 = CreateTestPerformance(0);
            Performance p2 = CreateTestPerformance(1);
            p2.VenueId = p1.VenueId;
            ps.UpdatePerformance(p2);
        }

        [TestMethod()]
        public void UpdatePerformanceWithValidInfo() {
            Performance p = CreateTestPerformance();
            p.Date = p.Date.AddDays(3);
            ps.UpdatePerformance(p);
        }
    }
}
