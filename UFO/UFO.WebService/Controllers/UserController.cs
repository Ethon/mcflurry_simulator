using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers {
    public class UserController : ApiController {
        private IUserService us;

        public UserController() {
            IDatabase db = DatabaseConnection.GetDatabase();
            us = ServiceFactory.CreateUserService(db);
        }

        public User GetUserById(uint id) {
            return us.GetUserById(id);
        }

        public User[] GetAllUsers() {
            return us.GetAllUsers().ToArray();
        }
        public bool GetCheckCredentials(string email, string password) {
            return us.CheckCredentials(email, password);
        }

        public User GetUserByEmailAdress(string email) {
            return us.GetUserByEmailAddress(email);
        }
    }
}