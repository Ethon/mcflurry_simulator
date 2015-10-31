using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class VenueDaoTests {
        private static IDatabase db;
        private IVenueDao vdao;
        private IDistrictDao ddao;
        private uint districtId, otherDistrictId;

        private List<Venue> GetTestVenueData() {
            return new List<Venue>() {
                new Venue(0, "Posthof", "PH", districtId, 134.567, 34.54),
                new Venue(0, "Musiktheater", "MT", districtId, 135.789, 45.67),
                new Venue(0, "Tischlerei", "T", districtId, 166.789, 39.67)
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
            ddao = new DistrictDao(db);
            districtId = ddao.CreateDistrict("Linz").Id;
            otherDistrictId = ddao.CreateDistrict("Auch Linz").Id;
        }

        [TestCleanup()]
        public void Cleanup() {
            vdao.DeleteAllVenues();
            ddao.DeleteAllDistricts();
        }

        [TestMethod()]
        public void CreateVenueTest() {
            var testvenues = GetTestVenueData();
            foreach (var cur in testvenues) {
                vdao.CreateVenue(cur.Name, cur.Shortcut, cur.DistrictId, cur.Latitude, cur.Longitude);
            }

            var allvenues = vdao.GetAllVenues();
            Assert.AreEqual(testvenues.Count, allvenues.Count);
        }

        [TestMethod()]
        public void DeleteAllVenuesTest() {
            var testvenues = GetTestVenueData();
            foreach (var cur in testvenues) {
                vdao.CreateVenue(cur.Name, cur.Shortcut, cur.DistrictId, cur.Latitude, cur.Longitude);
            }
            vdao.DeleteAllVenues();
            Assert.AreEqual(0, vdao.GetAllVenues().Count);
            vdao.DeleteAllVenues();
            Assert.AreEqual(0, vdao.GetAllVenues().Count);
        }

        [TestMethod()]
        public void DeleteVenueTest() {
            var testvenues = GetTestVenueData();
            Venue v1 = vdao.CreateVenue(testvenues[0].Name, testvenues[0].Shortcut, testvenues[0].DistrictId, testvenues[0].Latitude, testvenues[0].Longitude);
            Venue v2 = vdao.CreateVenue(testvenues[1].Name, testvenues[1].Shortcut, testvenues[1].DistrictId, testvenues[1].Latitude, testvenues[1].Longitude);
            var allvenues = vdao.GetAllVenues();
            Assert.AreEqual(2, allvenues.Count);
            vdao.DeleteVenue(v1);
            allvenues = vdao.GetAllVenues();
            Assert.AreEqual(1, allvenues.Count);
            Assert.IsNull(vdao.GetVenueById(v1.Id));
            Assert.IsNotNull(vdao.GetVenueById(v2.Id));
        }

        [TestMethod()]
        public void GetAllVenuesTest() {
            var testvenues = GetTestVenueData();
            for (int i = 0; i < testvenues.Count; ++i) {
                testvenues[i] = vdao.CreateVenue(testvenues[i].Name, testvenues[i].Shortcut, testvenues[i].DistrictId, testvenues[i].Latitude, testvenues[i].Longitude);
                Assert.IsNotNull(testvenues[i]);
            }

            var allvenues = vdao.GetAllVenues();
            Assert.AreEqual(testvenues.Count, allvenues.Count);
            for (int i = 0; i < allvenues.Count; ++i) {
                Assert.AreEqual(testvenues[i], allvenues[i]);
            }
        }

        [TestMethod()]
        public void GetVenueByIdTest() {
            var testvenues = GetTestVenueData();
            Venue v1 = vdao.CreateVenue(testvenues[0].Name, testvenues[0].Shortcut, testvenues[0].DistrictId, testvenues[0].Latitude, testvenues[0].Longitude);
            Venue v2 = vdao.GetVenueById(v1.Id);
            Assert.IsNotNull(v2);
            Assert.AreEqual(v1, v2);
            vdao.DeleteVenue(v1);
            Assert.IsNull(vdao.GetVenueById(v1.Id));
        }

        [TestMethod()]
        public void UpdateVenueTest() {
            var testvenues = GetTestVenueData();
            Venue v1 = vdao.CreateVenue(testvenues[0].Name, testvenues[0].Shortcut, testvenues[0].DistrictId, testvenues[0].Latitude, testvenues[0].Longitude);
            v1.Name = "othern";
            v1.Shortcut = "others";
            v1.DistrictId = otherDistrictId;
            v1.Latitude = 1000.2000;
            v1.Longitude = 3000.4000;
            vdao.UpdateVenue(v1);
            Venue c2 = vdao.GetVenueById(v1.Id);
            Assert.AreEqual(v1, c2);
        }
    }
}