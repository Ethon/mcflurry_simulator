using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class DistrictDaoTests {
        private static IDatabase db;
        private IDistrictDao ddao;

        private List<District> GetTestDistrictData() {
            return new List<District>() {
                new District(0, "Neustadt"),
                new District(0, "Altstadt"),
                new District(0, "Zentrum")
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
            ddao = new DistrictDao(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            ddao.DeleteAllDistricts();
        }

        [TestMethod()]
        public void CreateDistrictTest() {
            var testdis = GetTestDistrictData();
            foreach (var cur in testdis) {
                ddao.CreateDistrict(cur.Name);
            }

            var allusers = ddao.GetAllDistricts();
            Assert.AreEqual(testdis.Count, allusers.Count);
        }

        [TestMethod()]
        public void DeleteAllDistrictsTest() {
            var testdis = GetTestDistrictData();
            foreach(var cur in testdis) {
                ddao.CreateDistrict(cur.Name);
            }
            ddao.DeleteAllDistricts();
            Assert.AreEqual(0, ddao.GetAllDistricts().Count);
            ddao.DeleteAllDistricts();
            Assert.AreEqual(0, ddao.GetAllDistricts().Count);
        }

        [TestMethod()]
        public void DeleteDistrictTest() {
            var testdis = GetTestDistrictData();
            District d1 = ddao.CreateDistrict(testdis[0].Name);
            District d2 = ddao.CreateDistrict(testdis[1].Name);
            var alldis = ddao.GetAllDistricts();
            Assert.AreEqual(2, alldis.Count);
            ddao.DeleteDistrict(d1);
            alldis = ddao.GetAllDistricts();
            Assert.AreEqual(1, alldis.Count);
            Assert.IsNull(ddao.GetDistrictById(d1.Id));
            Assert.IsNotNull(ddao.GetDistrictById(d2.Id));
        }

        [TestMethod()]
        public void GetAllDistrictsTest() {
            var testdis = GetTestDistrictData();
            for (int i = 0; i < testdis.Count; ++i) {
                testdis[i] = ddao.CreateDistrict(testdis[i].Name);
                Assert.IsNotNull(testdis[i]);
            }

            var alldis = ddao.GetAllDistricts();
            Assert.AreEqual(testdis.Count, alldis.Count);
            for (int i = 0; i < alldis.Count; ++i) {
                Assert.AreEqual(testdis[i], alldis[i]);
            }
        }

        [TestMethod()]
        public void GetDistrictByIdTest() {
            var testdis = GetTestDistrictData();
            District d1 = ddao.CreateDistrict(testdis[0].Name);
            District d2 = ddao.GetDistrictById(d1.Id);
            Assert.IsNotNull(d2);
            Assert.AreEqual(d1, d2);
            ddao.DeleteDistrict(d1);
            Assert.IsNull(ddao.GetDistrictById(d1.Id));
        }

        [TestMethod()]
        public void UpdateDistrictTest() {
            var testdis = GetTestDistrictData();
            District d1 = ddao.CreateDistrict(testdis[0].Name);
            d1.Name = "otherName";
            ddao.UpdateDistrict(d1);
            District d2 = ddao.GetDistrictById(d1.Id);
            Assert.AreEqual(d1, d2);
        }
    }
}