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

        public Country GetCountryById(uint id)
        {
            return countryService.GetCountryById(id);
        }

        public Country[] GetAllCountries()
        {
            return countryService.GetAllCountries().ToArray();
        }

        public void UpdateCountry(Country country)
        {
            countryService.UpdateCountry(country);
        }

        public void DeleteCountry(Country country)
        {
            countryService.DeleteCountry(country);
        }

        public Country CreateCountry(String name, String flagPath)
        {
            return countryService.CreateCountry(name, flagPath);
        }
    }
}
