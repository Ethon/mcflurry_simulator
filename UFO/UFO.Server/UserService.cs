using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public interface IUserService {
        User GetUserById(uint id);
        User GetUserByEmailAddress(string email);
        bool CheckCredentials(string email, string password);
        List<User> GetAllUsers();
    }

    internal class UserService : IUserService {
        private static Regex nameRegex = new Regex("^[\\p{L} ]+$");
        private static Regex emailRegex = new Regex("^\\w+@\\w+.\\w+$");

        private IUserDao udao;

        private static bool IsValidName(string name) {
            return nameRegex.IsMatch(name);
        }

        private static bool IsValidEmail(string email) {
            return emailRegex.IsMatch(email);
        }

        public UserService(IDatabase db) {
            if (db is MYSQLDatabase) {
                udao = new UserDao(db);
            } else {
                throw new NotSupportedException("Database not supported");
            }
        }

        public User GetUserById(uint id) {
            return udao.GetUserById(id);
        }

        public User GetUserByEmailAddress(string email) {
            if (!IsValidEmail(email)) {
                throw new DataValidationException("Can't query user with invalid email '" + email + "'");
            }
            return udao.GetUserByEmailAddress(email);
        }

        public bool CheckCredentials(string email,string password) {
            var user = GetUserByEmailAddress(email);
            if (user == null) {
                return false;
                //throw new DataValidationException("Unknown user '" + email + "'");
            }
            if( user.Password != password) {
                //throw new DataValidationException("Wrong password for user '" + email + "'");
                return false;
            }
            return true;
        }

        public List<User> GetAllUsers() {
            return udao.GetAllUsers();
        }
    }
}
