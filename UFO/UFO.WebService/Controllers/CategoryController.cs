using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers {
    public class CategoryController : ApiController {
        private ICategoryService cs;

        public CategoryController() {
            IDatabase db = DatabaseConnection.GetDatabase();
            cs = ServiceFactory.CreateCategoryService(db);
        }

        [HttpGet]
        public Category GetCategoryById(uint id) {
            return cs.GetCategoryById(id);
        }

        [HttpGet]
        public Category[] GetAllCategories() {
            return cs.GetAllCategories().ToArray();
        }

    }
}