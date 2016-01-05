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
    class UserWebService : IUserService
    {
        private RestClient client;

        public UserWebService(RestClient client)
        {
            this.client = client;
        }

        public bool CheckCredentials(string email, string password)
        {
            var request = new RestRequest("api/User/CheckCredentials?email={email}&password={password}", Method.GET);
            request.AddUrlSegment("email", email);
            request.AddUrlSegment("password", password);
            return client.Execute<bool>(request).Data;
        }

        public List<User> GetAllUsers()
        {
            var request = new RestRequest("api/User/GetAllUsers", Method.GET);
            return client.Execute<List<User>>(request).Data;
        }

        public User GetUserByEmailAddress(string email)
        {
            var request = new RestRequest("api/User/GetUserByEmailAdress?email={email}", Method.GET);
            request.AddUrlSegment("email", email);
            return client.Execute<User>(request).Data;
        }

        public User GetUserById(uint id)
        {
            var request = new RestRequest("api/User/GetUserById/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return client.Execute<User>(request).Data;
        }
    }
}
