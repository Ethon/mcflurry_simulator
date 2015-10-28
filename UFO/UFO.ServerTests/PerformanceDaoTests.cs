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

        //private IVenueDao vdao;
        //private IArtistDao adao;


        //private List<Artist> GetTestArtistData() {
        //    return new List<Artist>() {
        //        new Artist(0,"Max Mustermann","max@mustermann.de",0,0,"0.jpg","0.mp4"),
        //        new Artist(1,"Theo Test", "test@test.at", 0, 1, "1.jpg", "1.mp4"),
        //        new Artist(2,"Hans Wurst", "hans@wurst.de", 2, 1, "2.jpg", "2.mp4"),
        //        new Artist(3,"Karla Tufo Group", "karla@tofu.com", 1, 1, "3.jpg", "3.mp4")
        //    };
        //}

        //private List<Venue> GetTestVenueData() {
        //    return new List<Venue>() {
        //        new Venue(0,"Hauptplatz","HP",1,12,12),
        //        new Venue(1,"Taubenmarkt","TM",1,3,8),
        //        new Venue(2,"OK-Platz","OP",2,3,3),
        //    };
        //}

        private List<Performance> GetTestPerformanceData() {

            //var testVenues = GetTestVenueData();
            //var testArtists = GetTestArtistData();

            //foreach (var item in testArtists) {
            //    adao.CreateArtist(item.Name, item.Email, item.CategoryId, item.CountryId, item.PicturePath, item.VideoPath);
            //}
            //foreach (var item in testVenues) {
            //    vdao.CreateVenue(item.Name, item.Shortcut, item.DistrictId, item.Latitude, item.Longitude);
            //}
            return new List<Performance>() {
                new Performance(0,new DateTime(2016,7,1,12,0,0),0,0),
                new Performance(1,new DateTime(2016,7,3,10,30,0),1,1),
                new Performance(2,new DateTime(2016,7,3,8,0,0),2,2),
                new Performance(3,new DateTime(2016,7,2,11,0,0),0,2),
                new Performance(4,new DateTime(2016,7,2,7,0,0),1,0)
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
            //vdao = new VenueDao(db);
            //adao = new ArtistDao(db);
            pdao = new PerformanceDao(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            pdao.DeleteAllPerformances();

            //vdao.DeleteAllVenues();
            //adao.DeleteAllArtists();
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
            p1.ArtistId = 2;
            p1.VenueId = 1;
            pdao.UpdatePerformance(p1);
            Performance p2 = pdao.GetPerformanceById(p1.Id);
            Assert.AreEqual(p1, p2);
        }
    }
}