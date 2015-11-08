using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public class CountryService {

        private ICountryDao couDao;
        private IArtistDao aDao;

        private static Regex nameRegex = new Regex("^[a-zA-Z]+$");

        public CountryService(IDatabase db) {
            couDao = new CountryDao(db);
            aDao = new ArtistDao(db);
        }

        private static bool IsValidName(string name) {
            return nameRegex.IsMatch(name);
        }
        private static bool IsValidFlagPath(string flagPath) {
            return true;
        }
        private bool IsUsedCountry(Country country) {
            return aDao.CountArtistsOfCountry(country) > 0;
        }

        public Country CreateCountry(string name, string flagPath) {
            if (!IsValidName(name)) {
                throw new DataValidationException ("Can't create country with invalid name '" + name + "'");
            }
            if (!IsValidFlagPath(flagPath)) {
                throw new DataValidationException("Can`t create country with invalid flagPath '" + flagPath + "'");
            }
            return couDao.CreateCountry(name, flagPath);
        }
        public Country GetCountryById(uint id) {
            return couDao.GetCountryById(id);
        }

        public List<Country> GetAllCountries() {
            return couDao.GetAllCountries();
        }

        public void UpdateCountry(Country country) {
            if (!IsValidName(country.Name)) {
                throw new DataValidationException("Can't update country to invalid name '" + country.Name + "'");
            } else if (!IsValidFlagPath(country.FlagPath)) {
                throw new DataValidationException("Can't update country to invalid flagpath '" + country.FlagPath+ "'");
            }
            if (!couDao.UpdateCountry(country)) {
                throw new DatabaseException("DatabaseError: Can`t update country " + country);
            }
        }

        public void DeleteCountry(Country country) {
            if (IsUsedCountry(country)) {
                throw new DataValidationException("Can't delete used country '" + country.Name + "'");
            }
            if (!couDao.DeleteCountry(country)) {
                throw new DatabaseException("DatabaseError: Can`t delete country " + country);
            }
        }
    }
}
