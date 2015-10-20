using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    class Program {
        static void Main(string[] args) {

            string providerName = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ProviderName;
            //DbProviderFactory dbFactory = DbProviderFactories.GetFactory(providerName);
            Console.WriteLine(providerName);
            IDatabase database = new MYSQLDatabase(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            Console.WriteLine("Using Database {0}", database.GetType());
    
            ICategoryDao catDao = new CategoryDao(database);
            /*
            catDao.CreateCategory("A", "Akrobatik");
            catDao.CreateCategory("C", "Comedy & Clownerie");
            catDao.CreateCategory("F", "Feuershow");
            catDao.CreateCategory("L", "Luftakrobatik");
            catDao.CreateCategory("M", "Musik");
            catDao.CreateCategory("J", "Jonglage");
            catDao.CreateCategory("OT", "Figuren - und Objekttheater");
            catDao.CreateCategory("S", "Samba");
            catDao.CreateCategory("ST", "Stehstill - Statue");*/
            //catDao.CreateCategory("WT", "wbhstiaall - Statue");
            Category category = catDao.GetCategoryById(22);
            if (category != null) {
                Console.WriteLine(category);
            }
            List<Category> categories = catDao.GetAllCategories();
            foreach (Category item in categories) {
                Console.WriteLine(item);
            }

            //catDao.DeleteCategory(category);
            category.Name = "Albert";
            category.Shortcut = "AL";
            Console.WriteLine(category);
            catDao.UpdateCategory(category);

            categories = catDao.GetAllCategories();
            foreach (Category item in categories) {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }
}
