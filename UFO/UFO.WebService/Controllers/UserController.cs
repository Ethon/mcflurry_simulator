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

        [HttpGet]
        public User GetUserById(uint id) {
            return us.GetUserById(id);
        }

        [HttpGet]
        public User[] GetAllUsers() {
            return us.GetAllUsers().ToArray();
        }

        [HttpGet]
        public bool CheckCredentials(string email, string password) {
            return us.CheckCredentials(email, password);
        }

        [HttpGet]
        public User GetUserByEmailAdress(string email) {
            return us.GetUserByEmailAddress(email);
        }
    }
}