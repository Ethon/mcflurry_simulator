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
            return String.Format("Category(id={0}, shortcut='{1}', name='{2}') ", Id,Shortcut, Name);
        }

        public override bool Equals(object obj) {
            Category c = obj as Category;
            if (c == null) {
                return false;
            }
            return Id.Equals(c.Id) && Shortcut.Equals(c.Shortcut) && Name.Equals(c.Name);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface ICategoryDao {
        List<Category> GetAllCategories();
        Category GetCategoryById(uint id);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        Category CreateCategory(string shortcut,string name);
        void DeleteAllCategories();
    }

    public class CategoryDao : ICategoryDao {

        private const string SQL_FIND_BY_ID = "SELECT * FROM Category WHERE categoryId=@categoryId";
        private const string SQL_FIND_ALL = "SELECT * FROM Category";
        private const string SQL_UPDATE = "UPDATE Category SET shortcut=@shortcut,name=@name WHERE categoryId=@categoryId";
        private const string SQL_INSERT = "INSERT INTO Category (shortcut,name) VALUES(@shortcut,@name)";
        private const string SQL_DELETE = "DELETE FROM Category WHERE categoryId=@categoryId";

        private IDatabase database;

        public CategoryDao(IDatabase database) {
            this.database = database;
        }

        public Category CreateCategory(string shortcut,string categoryName) {
            DbCommand cmd = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(cmd, "@shortcut", DbType.String, shortcut);
            database.DefineParameter(cmd, "@name", DbType.String, categoryName);
            int lastInsertID = database.ExecuteNonQuery(cmd);
            return new Category((uint)lastInsertID, shortcut, categoryName);
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
            using (IDataReader reader = database.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    Category newCat = new Category((uint)reader["categoryId"], (string)reader["shortcut"], (string)reader["name"]);
                    categories.Add(newCat);
                }
            }
            return categories;
        }

        public Category GetCategoryById(uint categoryId) {
            DbCommand cmd = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(cmd, "@categoryId", DbType.UInt32 , categoryId);
            using (IDataReader reader = database.ExecuteReader(cmd)) {
                if (reader.Read()) {
                    return new Category((uint)reader["categoryId"], (string)reader["shortcut"],
                        (string)reader["name"]);
                } else {
                    return null;
                }
            }
        }

        public bool UpdateCategory(Category category) {
            if (category != null) {
                DbCommand cmd = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(cmd, "@shortcut", DbType.String, category.Shortcut);
                database.DefineParameter(cmd, "@name", DbType.String, category.Name);
                database.DefineParameter(cmd, "@categoryId", DbType.String, category.Id);
                return database.ExecuteNonQuery(cmd) >= 1;
            }
            return false;
        }

        public void DeleteAllCategories() {
            database.TruncateTable("Category");
        }
    }

}
