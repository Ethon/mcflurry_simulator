using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class PerformanceDaoTests {
        private static IDatabase db;
        private IPerformanceDao pdao;

        private IVenueDao vdao;
        private IArtistDao adao;
        private ICategoryDao catDao;
        private ICountryDao couDao;
        private uint venueId, artistId, categoryId,countryId, otherVenueId, otherArtistId,otherCategoryId,otherCountryId;

        private List<Performance> GetTestPerformanceData() {
            return new List<Performance>() {
                new Performance(0,new DateTime(2016,7,1,12,0,0),categoryId,countryId),
                new Performance(1,new DateTime(2016,7,3,10,30,0),categoryId,countryId),
                new Performance(2,new DateTime(2016,7,3,8,0,0),categoryId,countryId),
                new Performance(3,new DateTime(2016,7,2,11,0,0),categoryId,countryId),
                new Performance(4,new DateTime(2016,7,2,7,0,0),categoryId,countryId)
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
            vdao = new VenueDao(db);
            adao = new ArtistDao(db);
            pdao = new PerformanceDao(db);
            catDao = new CategoryDao(db);
            couDao = new CountryDao(db);

            artistId = adao.CreateArtist("Max", "m@m.de", categoryId, countryId, "max.png", "max.mp4").Id;
            otherArtistId = adao.CreateArtist("Moritz", "mo@m.at", otherCategoryId, otherCountryId, "mo.png", "mo.mp4").Id;
            venueId = vdao.CreateVenue("Hauptplatz", "HP", 0, 1.234, 2.345).Id;
            otherVenueId = vdao.CreateVenue("Taubenmarkt", "TM", 0, 1.321,3.321).Id;
            categoryId = catDao.CreateCategory("TA", "Tanzakrobatik").Id;
            otherCategoryId = catDao.CreateCategory("MU", "Musik").Id;
            countryId = couDao.CreateCountry("Austria", "austria.png").Id;
            otherCountryId = couDao.CreateCountry("Germany", "germany.png").Id;
        }

        [TestCleanup()]
        public void Cleanup() {
            pdao.DeleteAllPerformances();
            vdao.DeleteAllVenues();
            adao.DeleteAllArtists();
            catDao.DeleteAllCategories();
            couDao.DeleteAllCountries();
        }

        [TestMethod()]
        public void CreatePerformanceTest() {
            
            var testPerformances = GetTestPerformanceData();
            
            foreach (var cur in testPerformances) {
                pdao.CreatePerformance(cur.Date, cur.ArtistId, cur.VenueId);
            }

            var allPerformances = pdao.GetAllPerformances();
            Assert.AreEqual(testPerformances.Count, allPerformances.Count);
           
        }

        [TestMethod()]
        public void DeleteAllPerformancesTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date,testPerformances[0].ArtistId,testPerformances[0].VenueId);
            Performance p2 = pdao.CreatePerformance(testPerformances[1].Date, testPerformances[1].ArtistId, testPerformances[1].VenueId);
            var allPerformances = pdao.GetAllPerformances();
            Assert.AreEqual(2, allPerformances.Count);
            pdao.DeletePerformance(p1);
            allPerformances = pdao.GetAllPerformances();
            Assert.AreEqual(1, allPerformances.Count);
            Assert.IsNull(pdao.GetPerformanceById(p1.Id));
            Assert.IsNotNull(pdao.GetPerformanceById(p2.Id));
        }

        [TestMethod()]
        public void DeletePerformanceTest() {
            var testPerformances = GetTestPerformanceData();

            foreach (var cur in testPerformances) {
                pdao.CreatePerformance(cur.Date,cur.ArtistId,cur.VenueId);
            }
            Assert.AreEqual(pdao.GetAllPerformances().Count, testPerformances.Count);
            pdao.DeleteAllPerformances();
            Assert.AreEqual(0, pdao.GetAllPerformances().Count);
        }

        [TestMethod()]
        public void GetAllPerformancesTest() {
            var testPerformances = GetTestPerformanceData();
            for (int i = 0; i < testPerformances.Count; ++i) {
                testPerformances[i] = pdao.CreatePerformance(testPerformances[i].Date,testPerformances[i].ArtistId,testPerformances[i].VenueId);
                Assert.IsNotNull(testPerformances[i]);
            }

            var allPerformances = pdao.GetAllPerformances();
            Assert.AreEqual(testPerformances.Count, allPerformances.Count);
            for (int i = 0; i < allPerformances.Count; ++i) {
                Assert.AreEqual(testPerformances[i], allPerformances[i]);
            }
        }

        [TestMethod()]
        public void GetPerformanceByIdTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date, testPerformances[0].ArtistId, testPerformances[0].VenueId);
            Performance p2 = pdao.GetPerformanceById(p1.Id);
            Assert.IsNotNull(p2);
            Assert.AreEqual(p1, p2);
            pdao.DeletePerformance(p1);
            Assert.IsNull(pdao.GetPerformanceById(p1.Id));
        }

        [TestMethod()]
        public void UpdatePerformanceTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date,testPerformances[0].ArtistId,testPerformances[0].VenueId);
            p1.Date = new DateTime(2016, 7, 12, 12, 0, 0);
            p1.ArtistId = otherArtistId;
            p1.VenueId = otherVenueId;
            pdao.UpdatePerformance(p1);
            Performance p2 = pdao.GetPerformanceById(p1.Id);
            Assert.AreEqual(p1, p2);
        }
    }
}