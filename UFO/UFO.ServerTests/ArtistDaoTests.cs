using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace Tests {
    [TestClass()]
    public class ArtistDaoTests {

        private static IDatabase db;
        private IArtistDao adao;

        private List<Artist> GetTestArtistData() {
            return new List<Artist>() {
                new Artist(0,"Max Mustermann","max@mustermann.de",0,0,"0.jpg","0.mp4"),
                new Artist(1,"Theo Test", "test@test.at", 0, 1, "1.jpg", "1.mp4"),
                new Artist(2,"Hans Wurst", "hans@wurst.de", 2, 1, "2.jpg", "2.mp4"),
                new Artist(3,"Karla Tufo Group", "karla@tofu.com", 1, 1, "3.jpg", "3.mp4")
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
            adao = new ArtistDao(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            adao.DeleteAllArtists();
        }


        [TestMethod()]
        public void CreateArtistTest() {
            var testArtists = GetTestArtistData();
            foreach (var cur in testArtists) {
                adao.CreateArtist(cur.Name, cur.Email, cur.CategoryId, cur.CountryId, cur.PicturePath, cur.VideoPath);
            }

            var allcountries = adao.GetAllArtists();
            Assert.AreEqual(testArtists.Count, allcountries.Count);
        }

        [TestMethod()]
        public void DeleteArtistTest() {
            var testArtists = GetTestArtistData();
            Artist a1 = adao.CreateArtist(testArtists[0].Name, testArtists[0].Email, testArtists[0].CategoryId, testArtists[0].CountryId, testArtists[0].PicturePath, testArtists[0].VideoPath);
            Artist a2 = adao.CreateArtist(testArtists[1].Name, testArtists[1].Email, testArtists[1].CategoryId, testArtists[1].CountryId, testArtists[1].PicturePath, testArtists[1].VideoPath);
            var allArtists = adao.GetAllArtists();
            Assert.AreEqual(2, allArtists.Count);
            adao.DeleteArtist(a1);
            allArtists = adao.GetAllArtists();
            Assert.AreEqual(1, allArtists.Count);
            Assert.IsNull(adao.GetArtistById(a1.Id));
            Assert.IsNotNull(adao.GetArtistById(a2.Id));
        }

        [TestMethod()]
        public void GetAllArtistsTest() {
            var testArtists = GetTestArtistData();
            for (int i = 0; i < testArtists.Count; ++i) {
                testArtists[i] = adao.CreateArtist(testArtists[i].Name, testArtists[i].Email, testArtists[i].CategoryId, testArtists[i].CountryId, testArtists[i].PicturePath, testArtists[i].VideoPath);
                Assert.IsNotNull(testArtists[i]);
            }

            var allartists = adao.GetAllArtists();
            Assert.AreEqual(testArtists.Count, allartists.Count);
            for (int i = 0; i < allartists.Count; ++i) {
                Assert.AreEqual(testArtists[i], allartists[i]);
            }
        }

        [TestMethod()]
        public void GetArtistByIdTest() {
            var testArtists = GetTestArtistData();
            Artist a1 = adao.CreateArtist(testArtists[0].Name, testArtists[0].Email, testArtists[0].CategoryId, testArtists[0].CountryId, testArtists[0].PicturePath, testArtists[0].VideoPath);
            Artist a2 = adao.GetArtistById(a1.Id);
            Assert.IsNotNull(a2);
            Assert.AreEqual(a1, a2);
            adao.DeleteArtist(a1);
            Assert.IsNull(adao.GetArtistById(a1.Id));
        }

        [TestMethod()]
        public void GetArtistByNameTest() {
            var testArtists = GetTestArtistData();
            Artist a1 = adao.CreateArtist(testArtists[0].Name, testArtists[0].Email, testArtists[0].CategoryId, testArtists[0].CountryId, testArtists[0].PicturePath, testArtists[0].VideoPath);
            Artist a2 = adao.GetArtistByName(a1.Name);
            Assert.IsNotNull(a2);
            Assert.AreEqual(a1, a2);
            adao.DeleteArtist(a1);
            Assert.IsNull(adao.GetArtistById(a1.Id));
        }

        [TestMethod()]
        public void UpdateArtistTest() {
            var testArtists = GetTestArtistData();
            Artist a1 = adao.CreateArtist(testArtists[0].Name, testArtists[0].Email, testArtists[0].CategoryId, testArtists[0].CountryId, testArtists[0].PicturePath, testArtists[0].VideoPath);
            a1.Name = "otherName";
            a1.Email = "otherMail";
            a1.CategoryId = 3;
            a1.CountryId = 5;
            a1.PicturePath = "newPath";
            a1.VideoPath = "newVPath";
            adao.UpdateArtist(a1);
            Artist a2 = adao.GetArtistById(a1.Id);
            Assert.AreEqual(a1, a2);
        }

        [TestMethod()]
        public void DeleteAllArtistsTest() {
            var testArtists = GetTestArtistData();

            foreach (var cur in testArtists) {
                adao.CreateArtist(cur.Name, cur.Email, cur.CategoryId, cur.CountryId, cur.PicturePath, cur.VideoPath);
            }
            Assert.AreEqual(adao.GetAllArtists().Count, testArtists.Count);
            adao.DeleteAllArtists();
            Assert.AreEqual(0, adao.GetAllArtists().Count);
        }
    }
}