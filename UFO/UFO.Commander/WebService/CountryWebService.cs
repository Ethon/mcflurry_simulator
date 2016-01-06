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
    class CountryWebService : ICountryService
    {
        private RestClient client;

        public CountryWebService(RestClient client)
        {
            this.client = client;
        }

        public Country CreateCountry(string name, string flagPath)
        {
            throw new NotImplementedException();
        }

        public void DeleteCountry(Country country)
        {
            throw new NotImplementedException();
        }

        public List<Country> GetAllCountries()
        {
            var request = new RestRequest("api/Country/GetAllCountries", Method.GET);
            return client.Execute<List<Country>>(request).Data;
        }

        public Country GetCountryById(uint id)
        {
            var request = new RestRequest("api/Country/GetCountryById/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return client.Execute<Country>(request).Data;
        }

        public void UpdateCountry(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
