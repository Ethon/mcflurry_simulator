﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public class UserService {
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

        public List<User> GetAllUsers() {
            return udao.GetAllUsers();
        }

        public void UpdateUser(User user) {
            if (!IsValidName(user.FirstName)) {
                throw new DataValidationException("Can't update user to invalid first name '" + user.FirstName + "'");
            } else if (!IsValidName(user.LastName)) {
                throw new DataValidationException("Can't update user to invalid last name '" + user.LastName + "'");
            } else if (!IsValidEmail(user.EmailAddress)) {
                throw new DataValidationException("Can't update user to invalid email '" + user.EmailAddress + "'");
            }
            if(!udao.UpdateUser(user)) {
                throw new DatabaseException("DatabaseError: Can`t update user " + user);
            }
        }

        public void DeleteUser(User user) {
            if (!udao.DeleteUser(user)) {
                throw new DatabaseException("DatabaseError: Can`t delete user " + user);
            }
        }
    }
}
