using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace UFO.Server.Data {
    public class User {
        public User(uint id, string firstName, string lastName, string email) {
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

        public override bool Equals(object obj) {
            User u = obj as User;
            if(u == null) {
                return false;
            }
            return Id.Equals(u.Id) && FirstName.Equals(u.FirstName) && LastName.Equals(u.LastName) && EmailAddress.Equals(u.EmailAddress);
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
        void UpdateUser(User user);
        void DeleteUser(User user);
        User CreateUser(string firstName, string lastName, string email);
    }

    public class UserDaoImpl : IUserDao {
        private const string DELETEALL_CMD = "TRUNCATE TABLE User";
        private const string CREATE_CMD = "INSERT INTO User(firstname, lastname, email) VALUES (@first, @last, @email)";
        private const string DELETE_CMD = "DELETE FROM User WHERE userId = @id";
        private const string GETALL_CMD = "SELECT * FROM User";
        private const string GETBYID_CMD = "SELECT * FROM User WHERE userId = @id";
        private const string GETBYEMAIL_CMD = "SELECT * FROM User WHERE email = @email";
        private const string UPDATE_CMD = "UPDATE User SET firstname=@first, lastname=@last, email=@email WHERE userId=@id";

        private IDatabase db;

        private User readOne(DbDataReader reader) {
            uint id = (uint)reader["userID"];
            string firstName = (string)reader["firstname"];
            string lastName = (string)reader["lastname"];
            string email = (string)reader["email"];
            return new User(id, firstName, lastName, email);
        }

        public UserDaoImpl(IDatabase db) {
            this.db = db;
        }

        public User CreateUser(string firstName, string lastName, string email) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@first", System.Data.DbType.String, firstName);
            db.DefineParameter(cmd, "@last", System.Data.DbType.String, lastName);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, email);
            int id = db.ExecuteNonQuery(cmd);
            return new User((uint)id, firstName, lastName, email);
        }

        public void DeleteUser(User user) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, user.Id);
            cmd.ExecuteNonQuery();
        }

        public List<User> GetAllUsers() {
            List<User> users = new List<User>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            using(DbDataReader reader = cmd.ExecuteReader()) {
                while(reader.Read()) {
                    users.Add(readOne(reader));
                }
            }
            return users;
        }

        public User GetUserByEmailAddress(string email) {
            DbCommand cmd = db.CreateCommand(GETBYEMAIL_CMD);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, email);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }

        public User GetUserById(uint id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }

        public void UpdateUser(User user) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, user.Id);
            db.DefineParameter(cmd, "@first", System.Data.DbType.String, user.FirstName);
            db.DefineParameter(cmd, "@last", System.Data.DbType.String, user.LastName);
            db.DefineParameter(cmd, "@email", System.Data.DbType.String, user.EmailAddress);
            db.ExecuteNonQuery(cmd);
        }

        public void DeleteAllUsers() {
            db.CreateCommand(DELETEALL_CMD).ExecuteNonQuery();
        }
    }
}
