using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public class CategoryService {

        private ICategoryDao catDao;
        private IArtistDao aDao;

        private static Regex shortcutRegex = new Regex("\\p{L}\\p{L}?");

        private static bool IsValidName(string name) {
            return true;
        }

        private static bool IsValidShortcut(string shortcut) {
            return shortcutRegex.IsMatch(shortcut) && shortcut.Length <= 2;
        }
        private bool IsUsedCategory(Category category) {
            return aDao.CountArtistsOfCategory(category) > 0;
        }

        public CategoryService(IDatabase db) {
            catDao = new CategoryDao(db);
            aDao = new ArtistDao(db);
        }

        public Category CreateCategory(string shortcut,string name) {
            if (!IsValidShortcut(shortcut)) {
                throw new DataValidationException("Can't create category with invalid shortcut '" + shortcut + "'");
            }
            if (!IsValidName(name)) {
                throw new DataValidationException("Can`t create category with invalid name '" + name + "'");
            }
            return catDao.CreateCategory(shortcut, name);
        }

        public List<Category> GetAllCategories() {
            return catDao.GetAllCategories();
        }

        public Category GetCategoryById(uint id) {
            return catDao.GetCategoryById(id);
        }

        public void UpdateCategory(Category category) {
            if (!IsValidShortcut(category.Shortcut)) {
                throw new DataValidationException("Can't update category with invalid shortcut '" + category.Shortcut + "'");
            }
            if (!IsValidName(category.Name)) {
                throw new DataValidationException("Can`t update category with invalid name '" + category.Name + "'");
            }
            if (!catDao.UpdateCategory(category)) {
                throw new DatabaseException("DatabaseError: Can`t update category " + category);
            };
        }

        public void DeleteCategory(Category category) {
            if (IsUsedCategory(category)) {
                throw new DataValidationException("Can't delete used category '" + category.Name + "'");
            }
            if (!catDao.DeleteCategory(category)) {
                throw new DatabaseException("DatabaseError: Can`t delete category " + category);
            };
        }
    }
}
