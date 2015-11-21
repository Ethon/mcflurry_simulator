using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public interface ICategoryService {
        List<Category> GetAllCategories();
        Category GetCategoryById(uint id);
    }

    internal class CategoryService : ICategoryService {
        private ICategoryDao catDao;

        public CategoryService(IDatabase db) {
            if (db is MYSQLDatabase) {
                catDao = new CategoryDao(db);
            } else {
                throw new NotSupportedException("Database not supported");
            }
        }
        public List<Category> GetAllCategories() {
            return catDao.GetAllCategories();
        }

        public Category GetCategoryById(uint id) {
            return catDao.GetCategoryById(id);
        }
    }
}
