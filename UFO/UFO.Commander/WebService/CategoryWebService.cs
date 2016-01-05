using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.WebService
{
    class CategoryWebService : ICategoryService
    {
        private RestClient client;

        public CategoryWebService(RestClient client)
        {
            this.client = client;
        }

        public List<Category> GetAllCategories()
        {
            return new List<Category>();
        }

        public Category GetCategoryById(uint id)
        {
            return new Category();
        }
    }
}
