using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class CountryDaoTests {
        private static IDatabase db;
        private ICountryDao cdao;

        private List<Country> GetTestCountryData() {
            return new List<Country>() {
                new Country(0, "Austria", "austria.png"),
                new Country(0, "Germany", "germany.png"),
                new Country(0, "Russia", "russia.png"),
                new Country(0, "United States Of America", "usa.png"),
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
            cdao = new CountryDao(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            cdao.DeleteAllCountries();
        }

        [TestMethod()]
        public void CreateCountryTest() {
            var testcountries = GetTestCountryData();
            foreach (var cur in testcountries) {
                cdao.CreateCountry(cur.Name, cur.FlagPath);
            }

            var allcountries = cdao.GetAllCountries();
            Assert.AreEqual(testcountries.Count, allcountries.Count);
        }

        [TestMethod()]
        public void GetAllCountriesTest() {
            var testcountries = GetTestCountryData();
            for (int i = 0; i < testcountries.Count; ++i) {
                testcountries[i] = cdao.CreateCountry(testcountries[i].Name, testcountries[i].FlagPath);
                Assert.IsNotNull(testcountries[i]);
            }

            var allcountries = cdao.GetAllCountries();
            Assert.AreEqual(testcountries.Count, allcountries.Count);
            for (int i = 0; i < allcountries.Count; ++i) {
                Assert.AreEqual(testcountries[i], allcountries[i]);
            }
        }

        [TestMethod()]
        public void GetCountryByIdTest() {
            var testcountries = GetTestCountryData();
            Country c1 = cdao.CreateCountry(testcountries[0].Name, testcountries[0].FlagPath);
            Country c2 = cdao.GetCountryById(c1.Id);
            Assert.IsNotNull(c2);
            Assert.AreEqual(c1, c2);
            cdao.DeleteCountry(c1);
            Assert.IsNull(cdao.GetCountryById(c1.Id));
        }

        [TestMethod()]
        public void UpdateCountryTest() {
            var testcountries = GetTestCountryData();
            Country c1 = cdao.CreateCountry(testcountries[0].Name, testcountries[0].FlagPath);
            c1.Name = "otherName";
            c1.FlagPath = "otherFlagpath";
            cdao.UpdateCountry(c1);
            Country c2 = cdao.GetCountryById(c1.Id);
            Assert.AreEqual(c1, c2);
        }

        [TestMethod()]
        public void DeleteCountryTest() {
            var testcountries = GetTestCountryData();
            Country c1 = cdao.CreateCountry(testcountries[0].Name, testcountries[0].FlagPath);
            Country c2 = cdao.CreateCountry(testcountries[1].Name, testcountries[1].FlagPath);
            var allcountries = cdao.GetAllCountries();
            Assert.AreEqual(2, allcountries.Count);
            cdao.DeleteCountry(c1);
            allcountries = cdao.GetAllCountries();
            Assert.AreEqual(1, allcountries.Count);
            Assert.IsNull(cdao.GetCountryById(c1.Id));
            Assert.IsNotNull(cdao.GetCountryById(c2.Id));
        }

        [TestMethod()]
        public void DeleteAllCountriesTest() {
            var testcountries = GetTestCountryData();
            foreach (var cur in testcountries) {
                cdao.CreateCountry(cur.Name, cur.FlagPath);
            }
            cdao.DeleteAllCountries();
            Assert.AreEqual(0, cdao.GetAllCountries().Count);
            cdao.DeleteAllCountries();
            Assert.AreEqual(0, cdao.GetAllCountries().Count);
        }
    }
}