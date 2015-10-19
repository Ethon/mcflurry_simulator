using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    [Serializable]
    public class Category {
        public Category() {

        }
        public Category(uint id, string shortcut, string name) {
            this.Id = id;
            this.Shortcut = shortcut;
            this.Name = name;
        }

        public uint Id {
            get; set;
        }

        public string Shortcut {
            get; set;
        }

        public string Name {
            get; set;
        }
        public override string ToString() {
            return Id+": ("+Shortcut+") "+Name;
        }
    }

    internal interface ICategoryDao {
        List<Category> GetAllCategories();
        Category GetCategoryById(uint id);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool CreateCategory(string shortcut,string name);
    }

    internal class CategoryDao : ICategoryDao {

        private const string SQL_FIND_BY_ID = "SELECT * FROM Category WHERE categoryId=@categoryId";
        private const string SQL_FIND_ALL = "SELECT * FROM Category";
        private const string SQL_UPDATE = "UPDATE Category SET shortcut=@shortcut,name=@name WHERE categoryId=@categoryId";
        private const string SQL_INSERT = "INSERT INTO Category (shortcut,name) VALUES(@shortcut,@name)";
        private const string SQL_DELETE = "DELETE FROM Category WHERE categoryId=@categoryId";

        private IDatabase database;

        public CategoryDao(IDatabase database) {
            this.database = database;
        }

        public bool CreateCategory(string shortcut,string categoryName) {
            DbCommand cmd = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(cmd, "@shortcut", DbType.String, shortcut);
            database.DefineParameter(cmd, "@name", DbType.String, categoryName);
            return database.ExecuteNonQuery(cmd) == 1;
        }

        public bool DeleteCategory(Category category) {
            if (category != null) {
                DbCommand cmd = database.CreateCommand(SQL_DELETE);
                database.DefineParameter(cmd, "@categoryId", DbType.UInt32,category.Id);
                return database.ExecuteNonQuery(cmd) == 1;
            }
            return false;
        }

        public List<Category> GetAllCategories() {
            DbCommand cmd = database.CreateCommand(SQL_FIND_ALL);
            List <Category> categories = new List<Category>();
            IDataReader reader = database.ExecuteReader(cmd);
            while (reader.Read()) {
                Category newCat = new Category((uint)reader["categoryId"], (string)reader["shortcut"], (string)reader["name"]);
                categories.Add(newCat);
            }
            return categories;
        }

        public Category GetCategoryById(uint categoryId) {
            DbCommand cmd = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(cmd, "@categoryId", DbType.UInt32 , categoryId);
            IDataReader reader = database.ExecuteReader(cmd);
            if (reader.Read()) {
                return new Category((uint)reader["categoryId"],(string)reader["shortcut"],
                    (string)reader["name"]);
            } else {
                return null;
            }
        }

        public bool UpdateCategory(Category category) {
            if (category != null) {
                DbCommand cmd = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(cmd, "@shortcut", DbType.String, category.Shortcut);
                database.DefineParameter(cmd, "@name", DbType.String, category.Name);
                database.DefineParameter(cmd, "@categoryId", DbType.String, category.Id);
                return database.ExecuteNonQuery(cmd) == 1;
            }
            return false;
        }
    }

}
