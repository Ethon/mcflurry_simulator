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
    public class UserServiceTests {

        private static IDatabase db;
        private static UserService us;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext) {
            db = new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;");
            us = new UserService(db);
        }

        [ClassCleanup()]
        public static void ClassCleanup() {
            us.DeleteAllUsers();
            db.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateUserWithInvalidFirstName() {
            us.CreateUser("123_", "Müller", "peter@mueller.com");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateUserWithInvalidLastName() {
            us.CreateUser("Peter", "123_", "peter@mueller.com");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void CreateUserWithInvalidEmail() {
            us.CreateUser("Peter", "Müller", "peter!mueller,com");
        }

        [TestMethod()]
        public void CreateUserWithValidInfo() {
            us.CreateUser("Peter", "Müller", "peter@mueller.com");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void GetUserWithInvalidEmail() {
            us.GetUserByEmailAddress("peter!mueller,com");
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void GetUserWithValidEmail() {
            Assert.IsNull(us.GetUserByEmailAddress("peter@mueller.com"));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateUserWithInvalidFirstName() {
            us.UpdateUser(new User(0, "123_", "Müller", "peter@mueller.com"));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateUserWithInvalidLastName() {
            us.UpdateUser(new User(0, "Peter", "123_", "peter@mueller.com"));
        }

        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void UpdateUserWithInvalidEmail() {
            us.UpdateUser(new User(0, "Peter", "Müller", "peter!mueller,com"));
        }

        [TestMethod()]
        public void UpdateUserWithValidInfo() {
            us.UpdateUser(new User(0, "Peter", "Müller", "peter@mueller.com"));
        }
    }
}