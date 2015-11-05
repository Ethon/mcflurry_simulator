using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public class UserService {
        private static Regex nameRegex = new Regex("[^\\w]+");
        private static Regex emailRegex = new Regex("\\w+@\\w+.\\w+");

        private UserDao udao;

        private static bool IsValidName(string name) {
            Match match = nameRegex.Match(name);
            return match.Success && match.Value.Equals(name);
        }

        private static bool IsValidEmail(string email) {
            return emailRegex.IsMatch(email);
        }

        public UserService(IDatabase db) {
            udao = new UserDao(db);
        }

        public User CreateUser(string firstName, string lastName, string email) {
            if(!IsValidName(firstName)) {
                throw new DataValidationException("Can't create user with invalid first name '" + firstName + "'");
            } else if(!IsValidName(lastName)) {
                throw new DataValidationException("Can't create user with invalid last name '" + lastName + "'");
            } else if(!IsValidEmail(email)) {
                throw new DataValidationException("Can't create user with invalid email '" + email + "'");
            }
            return udao.CreateUser(firstName, lastName, email);
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

        public void UpdateUser(User user) {
            if (!IsValidName(user.FirstName)) {
                throw new DataValidationException("Can't update user to invalid first name '" + user.FirstName + "'");
            } else if (!IsValidName(user.LastName)) {
                throw new DataValidationException("Can't update user to invalid last name '" + user.LastName + "'");
            } else if (!IsValidEmail(user.EmailAddress)) {
                throw new DataValidationException("Can't update user to invalid email '" + user.EmailAddress + "'");
            }
            udao.UpdateUser(user);
        }

        public void DeleteUser(User user) {
            udao.DeleteUser(user);
        }
    }
}
