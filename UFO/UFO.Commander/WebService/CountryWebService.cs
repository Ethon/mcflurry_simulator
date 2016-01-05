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
            return new Country();
        }

        public void DeleteCountry(Country country)
        {
            throw new NotImplementedException();
        }

        public List<Country> GetAllCountries()
        {
            return new List<Country>();
        }

        public Country GetCountryById(uint id)
        {
            return new Country();
        }

        public void UpdateCountry(Country country)
        {
            throw new NotImplementedException();
        }
    }
}
