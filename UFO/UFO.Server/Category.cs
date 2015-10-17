using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Category {
        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }
    }

    internal interface CategoryDao {
        List<Category> GetAllCategories();
        User GetCategoryById(uint id);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        uint CreateCategory(string name);
    }
}
