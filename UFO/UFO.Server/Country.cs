using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Country {
        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string FlagPath {
            get; set;
        }
    }
    internal interface ICountryDao {
        List<Country> GetAllCountries();
        User GetCountryById(uint id);
        void UpdateCountry(Country country);
        void DeleteCountry(Country country);
        uint CreateCountry(string name, string flagPath);
    }
}
