using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace UFO.Server.Data {
    public class User {
        public User(uint id, string firstName, string lastName, string email, string password) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = email;
            Password = password;
        }

        public uint Id {
            get; private set;
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
        public string Password
        {
            get; set;
        }
        
        public override string ToString() {
            return String.Format("User(id={0}, firstname='{1}', lastname='{2}', email='{3}', password='{4}') ", Id, FirstName, LastName, EmailAddress, Password);
        }

        public override bool Equals(object obj) {
            User u = obj as User;
            if(u == null) {
                return false;
            }
            return Id.Equals(u.Id) && FirstName.Equals(u.FirstName) && LastName.Equals(u.LastName) && EmailAddress.Equals(u.EmailAddress) && Password.Equals(u.Password);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface IUserDao {
        void DeleteAllUsers();
        List<User> GetAllUsers();
        User GetUserById(uint id);
        User GetUserByEmailAddress(string email);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        User CreateUser(string firstName, string lastName, string email, string password);
    }

    public class UserDao : IUserDao {
        private const string CREATE_CMD = "INSERT INTO User(firstname, lastname, email, password) VALUES (@first, @last, @email,@password)";
        private const string DELETE_CMD = "DELETE FROM User WHERE userId = @id";
        private const string GETALL_CMD = "SELECT * FROM User";
        private const string GETBYID_CMD = "SELECT * FROM User WHERE userId = @id";
        private const string GETBYEMAIL_CMD = "SELECT * FROM User WHERE email = @email";
        private const string UPDATE_CMD = "UPDATE User SET firstname=@first, lastname=@last, email=@email, password=@password WHERE userId=@id";

        private IDatabase db;

        private User readOne(IDataReader reader) {
            uint id = (uint)reader["userID"];
            string firstName = (string)reader["firstname"];
            string lastName = (string)reader["lastname"];
            string email = (string)reader["email"];
            string password = (string)reader["password"];
            return new User(id, firstName, lastName, email, password);
        }

        public UserDao(IDatabase db) {
            this.db = db;
        }

        public User CreateUser(string firstName, string lastName, string email, string password) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@first", System.Data.DbType.String, firstName);
            db.DefineParameter(cmd, "@last", System.Data.DbType.String, lastName);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, email);
            db.DefineParameter(cmd, "@password", System.Data.DbType.String, password);
            int id = db.ExecuteNonQuery(cmd);
            return new User((uint)id, firstName, lastName, email,password);
        }

        public bool DeleteUser(User user) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, user.Id);
            return cmd.ExecuteNonQuery() >= 1;
        }

        public List<User> GetAllUsers() {
            List<User> users = new List<User>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            db.doSynchronized(() => {
                using (IDataReader reader = db.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        users.Add(readOne(reader));
                    }
                }
            });
            return users;
        }

        public User GetUserByEmailAddress(string email) {
            DbCommand cmd = db.CreateCommand(GETBYEMAIL_CMD);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, email);
            User user = null;
            db.doSynchronized(() => {
                using (IDataReader reader = db.ExecuteReader(cmd)) {
                    if (reader.Read()) {
                        user = readOne(reader);
                    }
                }
            });
            return user;
        }

        public User GetUserById(uint id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, id);
            User user = null;
            db.doSynchronized(() => {
                using (IDataReader reader = db.ExecuteReader(cmd)) {
                    if (reader.Read()) {
                        user = readOne(reader);
                    }
                }
            });
            return user;
        }

        public bool UpdateUser(User user) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, user.Id);
            db.DefineParameter(cmd, "@first", System.Data.DbType.String, user.FirstName);
            db.DefineParameter(cmd, "@last", System.Data.DbType.String, user.LastName);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, user.EmailAddress);
            db.DefineParameter(cmd, "@password", System.Data.DbType.String, user.Password);
            return db.ExecuteNonQuery(cmd) >= 1;
        }

        public void DeleteAllUsers() {
            db.TruncateTable("User");
        }
    }
}
