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
        private static IUserDao udao;
        private static IUserService us;

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
            udao = new UserDao(db);
            us = ServiceFactory.CreateUserService(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            udao.DeleteAllUsers();
        }
        [TestMethod()]
        [ExpectedException(typeof(DataValidationException))]
        public void GetUserWithInvalidEmail() {
            us.GetUserByEmailAddress("peter!mueller,com");
        }

        [TestMethod()]
        public void GetUserWithValidEmail() {
            Assert.IsNull(us.GetUserByEmailAddress("peter@mueller.com"));
        }
    }
}