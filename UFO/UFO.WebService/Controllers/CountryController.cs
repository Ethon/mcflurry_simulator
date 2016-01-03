using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers
{
    public class CountryController : ApiController {
        private ICountryService cs;

        public CountryController()
        {
            IDatabase db = new MYSQLDatabase("Server = localhost; Database = ufo; Uid = root;");
            cs = ServiceFactory.CreateCountryService(db);
        }

        public Country GetCountryById(uint id)
        {
            return cs.GetCountryById(id);
        }

        public Country[] GetAllCountries()
        {
            return cs.GetAllCountries().ToArray();
        }

        public void UpdateCountry(Country country)
        {
            cs.UpdateCountry(country);
        }

        public void DeleteCountry(Country country)
        {
            cs.DeleteCountry(country);
        }

        public Country CreateCountry(String name, String flagPath)
        {
            return cs.CreateCountry(name, flagPath);
        }
    }
}
