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
        private ICountryService countryService;

        public CountryController()
        {
            countryService = ServiceFactory.CreateCountryService(DatabaseConnection.GetDatabase());
        }

        [HttpGet]
        public Country GetCountryById(uint id)
        {
            return countryService.GetCountryById(id);
        }

        [HttpGet]
        public Country[] GetAllCountries()
        {
            return countryService.GetAllCountries().ToArray();
        }

        [HttpPost]
        public void UpdateCountry(Country country)
        {
            countryService.UpdateCountry(country);
        }

        [HttpPost]
        public void DeleteCountry(Country country)
        {
            countryService.DeleteCountry(country);
        }

        [HttpGet]
        public Country CreateCountry(String name, String flagPath)
        {
            return countryService.CreateCountry(name, flagPath);
        }
    }
}
