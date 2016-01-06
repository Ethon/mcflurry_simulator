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
            var request = new RestRequest("api/Category/GetAllCategories", Method.GET);
            return client.Execute<List<Category>>(request).Data;
        }

        public Category GetCategoryById(uint id)
        {
            var request = new RestRequest("api/Category/GetCategoryById/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return client.Execute<Category>(request).Data;
        }
    }
}
