using Microsoft.VisualStudio.TestTools.UnitTesting;
using UFO.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data.Tests {
    [TestClass()]
    public class UserDaoImplTests {

        private static IDatabase db;
        private IUserDao udao;

        private List<User> GetTestUserData() {
            return new List<User>() {
                new User(0, "Max", "Mustermann", "max.mustermann@gmail.com"),
                new User(0, "Karla", "Tofu", "karla@tofu.at"),
                new User(0, "Hans", "Wurst", "hanswurst@gmx.at"),
                new User(0, "Döner", "Dürüm", "doener@hotmail.de")
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
            udao = new UserDaoImpl(db);
        }

        [TestCleanup()]
        public void Cleanup() {
            udao.DeleteAllUsers();
        }

        [TestMethod()]
        public void CreateUserTest() {
            var testusers = GetTestUserData();
            foreach(var user in testusers) {
                udao.CreateUser(user.FirstName, user.LastName, user.EmailAddress);
            }

            var allusers = udao.GetAllUsers();
            Assert.AreEqual(testusers.Count, allusers.Count);
        }

        [TestMethod()]
        public void DeleteUserTest() {
            var testusers = GetTestUserData();
            User u1 = udao.CreateUser(testusers[0].FirstName, testusers[0].LastName, testusers[0].EmailAddress);
            User u2 = udao.CreateUser(testusers[1].FirstName, testusers[1].LastName, testusers[1].EmailAddress);
            var allusers = udao.GetAllUsers();
            Assert.AreEqual(2, allusers.Count);
            udao.DeleteUser(u1);
            allusers = udao.GetAllUsers();
            Assert.AreEqual(1, allusers.Count);
            Assert.IsNull(udao.GetUserById(u1.Id));
            Assert.IsNotNull(udao.GetUserById(u2.Id));
        }

        [TestMethod()]
        public void GetAllUsersTest() {
            var testusers = GetTestUserData();
            for(int i = 0; i < testusers.Count; ++i) {
                testusers[i] = udao.CreateUser(testusers[i].FirstName, testusers[i].LastName, testusers[i].EmailAddress);
                Assert.IsNotNull(testusers[i]);
            }

            var allusers = udao.GetAllUsers();
            Assert.AreEqual(testusers.Count, allusers.Count);
            for(int i = 0; i < allusers.Count; ++i) {
                Assert.AreEqual(testusers[i], allusers[i]);
            }
        }

        [TestMethod()]
        public void GetUserByEmailAddressTest() {
            var testusers = GetTestUserData();
            User u1 = udao.CreateUser(testusers[0].FirstName, testusers[0].LastName, testusers[0].EmailAddress);
            User u2 = udao.GetUserByEmailAddress(u1.EmailAddress);
            Assert.IsNotNull(u2);
            Assert.AreEqual(u1, u2);
            udao.DeleteUser(u1);
            Assert.IsNull(udao.GetUserByEmailAddress(u1.EmailAddress));
        }

        [TestMethod()]
        public void GetUserByIdTest() {
            var testusers = GetTestUserData();
            User u1 = udao.CreateUser(testusers[0].FirstName, testusers[0].LastName, testusers[0].EmailAddress);
            User u2 = udao.GetUserById(u1.Id);
            Assert.IsNotNull(u2);
            Assert.AreEqual(u1, u2);
            udao.DeleteUser(u1);
            Assert.IsNull(udao.GetUserById(u1.Id));
        }

        [TestMethod()]
        public void UpdateUserTest() {
            var testusers = GetTestUserData();
            User u1 = udao.CreateUser(testusers[0].FirstName, testusers[0].LastName, testusers[0].EmailAddress);
            u1.FirstName = "otherFirst";
            u1.LastName = "otherLast";
            u1.EmailAddress = "other@mail.com";
            udao.UpdateUser(u1);
            User u2 = udao.GetUserById(u1.Id);
            Assert.AreEqual(u1, u2);
        }

        [TestMethod()]
        public void DeleteAllUsersTest() {
            var testusers = GetTestUserData();
            User u1 = udao.CreateUser(testusers[0].FirstName, testusers[0].LastName, testusers[0].EmailAddress);
            User u2 = udao.CreateUser(testusers[1].FirstName, testusers[1].LastName, testusers[1].EmailAddress);
            User u3 = udao.CreateUser(testusers[2].FirstName, testusers[2].LastName, testusers[2].EmailAddress);
            User u4 = udao.CreateUser(testusers[3].FirstName, testusers[3].LastName, testusers[3].EmailAddress);
            udao.DeleteAllUsers();
            Assert.AreEqual(0, udao.GetAllUsers().Count);
            udao.DeleteAllUsers();
            Assert.AreEqual(0, udao.GetAllUsers().Count);
        }
    }
}