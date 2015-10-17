using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UFO.Server.Data {
    public class User {
        internal User(uint id, string firstName, string lastName, string email) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = email;
        }

        public uint Id {
            get; set;
        }

        public string FirstName {
            get; set;
        }

        public string LastName {
            get; set;
        }

        public string EmailAddress {
            get; set;
        }
        
        public override string ToString() {
            return String.Format("User(id={0}, firstname='{1}', lastname='{2}', email='{3}') ", Id, FirstName, LastName, EmailAddress);
        }
    }

    internal interface UserDao {
        List<User> GetAllUsers();
        User GetUserById(uint id);
        void UpdateUser(User user);
        void DeleteUser(User user);
        uint CreateUser(string firstName, string lastName, string email);
    }

    internal class UserDaoImpl : UserDao {
        private MySqlConnection con;

        public UserDaoImpl(MySqlConnection con) {
            this.con = con;
        }

        public uint CreateUser(string firstName, string lastName, string email) {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO User(firstname, lastname, email) VALUES (@first, @last, @email)", con);
            cmd.Parameters.Add("@first", MySqlDbType.String).Value = firstName;
            cmd.Parameters.Add("@last", MySqlDbType.String).Value = lastName;
            cmd.Parameters.Add("@email", MySqlDbType.String).Value = email;
            cmd.ExecuteNonQuery();
            return (uint)cmd.LastInsertedId;
        }

        public void DeleteUser(User user) {
            MySqlCommand cmd = new MySqlCommand("DELETE FROM User WHERE userId = " + user.Id, con);
            cmd.ExecuteNonQuery();
        }

        public List<User> GetAllUsers() {
            List<User> users = new List<User>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM User", con);
            using(MySqlDataReader reader = cmd.ExecuteReader()) {
                while(reader.Read()) {
                    uint id = (uint)reader["userID"];
                    string firstName = (string)reader["firstname"];
                    string lastName = (string)reader["lastname"];
                    string email = (string)reader["email"];
                    users.Add(new User(id, firstName, lastName, email));
                }
            }
            return users;
        }

        public User GetUserById(uint id) {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM User WHERE userId = " + id, con);
            using (MySqlDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    string firstName = (string)reader["firstname"];
                    string lastName = (string)reader["lastname"];
                    string email = (string)reader["email"];
                    return new User(id, firstName, lastName, email);
                } else {
                    return null;
                }
            }
        }

        public void UpdateUser(User user) {
            throw new NotImplementedException();
        }
    }
}
