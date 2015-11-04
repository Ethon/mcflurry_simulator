using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {

    public class Country {
        public Country(uint id, string name, string flagPath) {
            this.Id = id;
            this.Name = name;
            this.FlagPath = flagPath;
        }

        public uint Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string FlagPath {
            get; set;
        }

        public override string ToString() {
            return String.Format("Country(id={0}, name='{1}', flagPath='{2}') ", Id, Name, FlagPath);
        }

        public override bool Equals(object obj) {
            Country u = obj as Country;
            if (u == null) {
                return false;
            }
            return Id.Equals(u.Id) && Name.Equals(u.Name) && FlagPath.Equals(u.FlagPath);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface ICountryDao {
        List<Country> GetAllCountries();
        Country GetCountryById(uint id);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        Country CreateCountry(string name, string flagPath);
        void DeleteAllCountries();
    }

    public class CountryDao : ICountryDao {
        private const string SQL_FIND_BY_ID = "SELECT * FROM Country WHERE countryId=@countryId";
        private const string SQL_FIND_ALL = "SELECT * FROM Country";
        private const string SQL_UPDATE = "UPDATE Country SET name=@name,flagPath=@flagPath WHERE countryId=@countryId";
        private const string SQL_INSERT = "INSERT INTO Country (name,flagPath) VALUES(@name,@flagPath)";
        private const string SQL_DELETE = "DELETE FROM Country WHERE countryId=@countryId";

        private IDatabase database;

        public CountryDao(IDatabase database) {
            this.database = database;
        }

        public Country CreateCountry(string name, string flagPath) {
            DbCommand cmd = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(cmd, "@name", DbType.String, name);
            database.DefineParameter(cmd, "@flagPath", DbType.String, flagPath);
            int lastInsertID = database.ExecuteNonQuery(cmd);
            return new Country((uint)lastInsertID, name, flagPath);
        }

        public List<Country> GetAllCountries() {
            List<Country> countries = new List<Country>();
            DbCommand cmd = database.CreateCommand(SQL_FIND_ALL);
            using (IDataReader reader = database.ExecuteReader(cmd)) {
                while (reader.Read()) {
                    Country newCountry = new Country((uint)reader["countryId"], (string)reader["name"], (string)reader["flagPath"]);
                    countries.Add(newCountry);
                }
            }
            return countries;
        }

        public Country GetCountryById(uint id) {
            DbCommand cmd = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(cmd, "@countryId", DbType.UInt32,id);
            using (IDataReader reader = database.ExecuteReader(cmd)) {
                if (reader.Read()) {
                    Console.WriteLine("FOUND");
                    return new Country((uint)reader["countryId"], (string)reader["name"], (string)reader["flagPath"]);
                } else {
                    return null;
                }
            }
        }

        public bool UpdateCountry(Country country) {
            if (country != null) {
                DbCommand cmd = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(cmd, "@name", DbType.String, country.Name);
                database.DefineParameter(cmd, "@flagPath", DbType.String, country.FlagPath);
                database.DefineParameter(cmd, "@countryId", DbType.String, country.Id);
                return database.ExecuteNonQuery(cmd) == 1;
            }
            return false;
        }

        public bool DeleteCountry(Country country) {
            if (country != null) {
                DbCommand cmd = database.CreateCommand(SQL_DELETE);
                database.DefineParameter(cmd, "@countryId", DbType.UInt32, country.Id);
                return database.ExecuteNonQuery(cmd) == 1;
            }
            return false;
        }

        public void DeleteAllCountries() {
            database.TruncateTable("Country");
        }
    }
}
