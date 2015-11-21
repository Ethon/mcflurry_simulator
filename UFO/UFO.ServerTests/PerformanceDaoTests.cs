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

        private List<Performance> GetTestPerformanceData() {
            return RepresentativeData.GetDefaultPerformances();
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
            catDao = new CategoryDao(db);
            couDao = new CountryDao(db);
            adao = new ArtistDao(db);
            pdao = new PerformanceDao(db);

            pdao.DeleteAllPerformances();
            vdao.DeleteAllVenues();
            adao.DeleteAllArtists();
            catDao.DeleteAllCategories();
            couDao.DeleteAllCountries();

            foreach (var item in RepresentativeData.GetDefaultVenues()) {
                vdao.CreateVenue(item.Name, item.Shortcut, item.Latitude, item.Longitude);
            }
            foreach (var item in RepresentativeData.GetDefaultCategories()) {
                catDao.CreateCategory(item.Shortcut, item.Name);
            }
            foreach (var item in RepresentativeData.GetDefaultCountries()) {
                couDao.CreateCountry(item.Name, item.FlagPath);
            }

            foreach (var item in RepresentativeData.GetDefaultArtists()) {
                adao.CreateArtist(item.Name, item.Email, item.CategoryId,
                    item.CountryId, item.PicturePath, item.VideoPath);
            }

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
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date,
                testPerformances[0].ArtistId, testPerformances[0].VenueId);
            Performance p2 = pdao.CreatePerformance(testPerformances[1].Date,
                testPerformances[1].ArtistId, testPerformances[1].VenueId);
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
                pdao.CreatePerformance(cur.Date, cur.ArtistId, cur.VenueId);
            }
            Assert.AreEqual(pdao.GetAllPerformances().Count, testPerformances.Count);
            pdao.DeleteAllPerformances();
            Assert.AreEqual(0, pdao.GetAllPerformances().Count);
        }

        [TestMethod()]
        public void GetAllPerformancesTest() {
            var testPerformances = GetTestPerformanceData();
            for (int i = 0; i < testPerformances.Count; ++i) {
                testPerformances[i] = pdao.CreatePerformance(testPerformances[i].Date,
                    testPerformances[i].ArtistId, testPerformances[i].VenueId);
                Assert.IsNotNull(testPerformances[i]);
            }

            var allPerformances = pdao.GetAllPerformances();
            Assert.AreEqual(testPerformances.Count, allPerformances.Count);
            for (int i = 0; i < allPerformances.Count; ++i) {
                Assert.AreEqual(testPerformances[i], allPerformances[i]);
            }
        }

        [TestMethod()]
        public void GetPerformancesByArtistBeforeDateTest() {
            var testPerformances = GetTestPerformanceData();
            Category cat = catDao.CreateCategory("TA", "Turnakrobatik");
            Country cou = couDao.CreateCountry("Wien", "wien.png");
            Artist a1 = adao.CreateArtist("Max", "max@max.de", cat.Id,  cou.Id , "max.png", "max.video");
            Artist a2 = adao.CreateArtist("Hermann Maier", "hermann@max.de", cat.Id, cou.Id, "max.png", "max.video");
            Venue v = vdao.CreateVenue("Hauptplatz", "HP", 1.23456, 1.2346);

            //Main Artist Before
            Performance pBefore1 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 12, 0, 0), a1.Id, v.Id);
            Performance pBefore2 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 15, 0, 0), a1.Id, v.Id);
            Performance pBefore3 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 18, 0, 0), a1.Id, v.Id);
            Performance pBefore4 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 20, 0, 0), a1.Id, v.Id);

            //Main Artist After
            Performance pAfter1 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 15, 0, 0), a1.Id, v.Id);
            Performance pAfter2 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 19, 0, 0), a1.Id, v.Id);
            Performance pAfter3 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 20, 0, 0), a1.Id, v.Id);

            //Other Artist
            Performance pOtherBefore = pdao.CreatePerformance(new DateTime(2016, 1, 1, 12, 0, 0), a2.Id, v.Id);
            Performance pOtherAfter = pdao.CreatePerformance(new DateTime(2016, 1, 2, 20, 0, 0), a2.Id, v.Id);

            Assert.AreEqual(4, pdao.GetPerformancesByArtistBeforeDate(a1.Id, new DateTime(2016, 1, 1, 22, 0, 0)).Count);
        }

        [TestMethod()]
        public void GetPerformancesByArtistAfterDateTest() {
            var testPerformances = GetTestPerformanceData();
            Category cat = catDao.CreateCategory("TA", "Turnakrobatik");
            Country cou = couDao.CreateCountry("Wien", "wien.png");
            Artist a1 = adao.CreateArtist("Max", "max@max.de", cat.Id, cou.Id, "max.png", "max.video");
            Artist a2 = adao.CreateArtist("Hermann Maier", "hermann@max.de", cat.Id, cou.Id, "max.png", "max.video");
            Venue v = vdao.CreateVenue("Hauptplatz", "HP", 1.23456, 1.2346);

            //Main Artist Before
            Performance pBefore1 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 12, 0, 0), a1.Id, v.Id);
            Performance pBefore2 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 15, 0, 0), a1.Id, v.Id);
            Performance pBefore3 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 18, 0, 0), a1.Id, v.Id);
            Performance pBefore4 = pdao.CreatePerformance(new DateTime(2016, 1, 1, 20, 0, 0), a1.Id, v.Id);

            //Main Artist After
            Performance pAfter1 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 15, 0, 0), a1.Id, v.Id);
            Performance pAfter2 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 19, 0, 0), a1.Id, v.Id);
            Performance pAfter3 = pdao.CreatePerformance(new DateTime(2016, 1, 2, 20, 0, 0), a1.Id, v.Id);

            //Other Artist
            Performance pOtherBefore = pdao.CreatePerformance(new DateTime(2016, 1, 1, 12, 0, 0), a2.Id, v.Id);
            Performance pOtherAfter = pdao.CreatePerformance(new DateTime(2016, 1, 2, 20, 0, 0), a2.Id, v.Id);

            Assert.AreEqual(3, pdao.GetPerformancesByArtistAfterDate(a1.Id, new DateTime(2016, 1, 1, 22, 0, 0)).Count);
        }


        [TestMethod()]
        public void GetPerformanceByIdTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date,
                testPerformances[0].ArtistId, testPerformances[0].VenueId);
            Performance p2 = pdao.GetPerformanceById(p1.Id);
            Assert.IsNotNull(p2);
            Assert.AreEqual(p1, p2);
            pdao.DeletePerformance(p1);
            Assert.IsNull(pdao.GetPerformanceById(p1.Id));
        }

        [TestMethod()]
        public void UpdatePerformanceTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p1 = pdao.CreatePerformance(testPerformances[0].Date,
                testPerformances[0].ArtistId, testPerformances[0].VenueId);
            p1.Date = new DateTime(2016, 7, 12, 12, 0, 0);
            p1.ArtistId = 20;
            p1.VenueId = 3;
            pdao.UpdatePerformance(p1);
            Performance p2 = pdao.GetPerformanceById(p1.Id);
            Assert.AreEqual(p1, p2);
        }

        [TestMethod()]
        public void CountOfPerformancesAtVenueTest() {
            var testPerformances = GetTestPerformanceData();
            foreach (var cur in testPerformances) {
                pdao.CreatePerformance(cur.Date, cur.ArtistId, cur.VenueId);
            }

            Dictionary<uint, uint> venueIdToCountMapping = new Dictionary<uint, uint>();
            foreach (var cur in testPerformances) {
                if (!venueIdToCountMapping.ContainsKey(cur.VenueId)) {
                    venueIdToCountMapping.Add(cur.VenueId, 1);
                } else {
                    ++venueIdToCountMapping[cur.VenueId];
                }
            }

            foreach (var pair in venueIdToCountMapping) {
                Assert.AreEqual(pair.Value,
                    pdao.CountOfPerformancesAtVenue(new Venue(pair.Key, null, null, 0, 0)));
            }
        }

        [TestMethod()]
        public void GetPerformanceByVenueAndDateTest() {
            var testPerformances = GetTestPerformanceData();
            Performance p = pdao.CreatePerformance(testPerformances[0].Date, testPerformances[0].ArtistId, testPerformances[0].VenueId);
            Venue v = new Venue(testPerformances[0].VenueId, null, null, 0, 0);
            Assert.IsNotNull(pdao.GetPerformanceByVenueAndDate(v.Id, testPerformances[0].Date));
            pdao.DeletePerformance(p);
            Assert.IsNull(pdao.GetPerformanceByVenueAndDate(v.Id, testPerformances[0].Date));
        }
    }
}