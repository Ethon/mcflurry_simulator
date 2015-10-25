using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Venue {
        public Venue(string id, string name, string shortcut, uint districtId, double lat, double lng) {
            Id = id;
            Name = name;
            Shortcut = shortcut;
            DistrictId = districtId;
            Latitude = lat;
            Longitude = lng;
        }

        public string Id {
            get; set;
        }

        public string Name {
            get; set;
        }

        public string Shortcut {
            get; set;
        }

        public uint DistrictId {
            get; set;
        }

        public double Latitude {
            get; set;
        }

        public double Longitude {
            get; set;
        }

        public override string ToString() {
            return String.Format("Venue(id={0}, name='{1}', shortcut='{2}', districtId={3}, latitude={4}, longitude={5})",
                Id, Name, Shortcut, DistrictId, Latitude, Longitude);
        }

        public override bool Equals(object obj) {
            Venue ven = obj as Venue;
            if (ven == null) {
                return false;
            }
            return  Id.Equals(ven.Id) &&
                    Name.Equals(ven.Name) &&
                    Shortcut.Equals(ven.Shortcut) &&
                    DistrictId.Equals(ven.DistrictId) &&
                    Latitude.Equals(ven.Latitude) &&
                    Longitude.Equals(ven.Longitude);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }

    public interface IVenueDao {
        void DeleteAllVenues();
        List<Venue> GetAllVenues();
        Venue GetVenueById(string id);
        void UpdateVenue(Venue ven);
        void DeleteVenue(Venue ven);
        Venue CreateVenue(string id, string name, string shortcut, uint districtId, double latitude, double longitude);
    }

    public class VenueDao : IVenueDao {
        private const string DELETEALL_CMD = "TRUNCATE TABLE Venue";
        private const string CREATE_CMD = "INSERT INTO Venue(venueId, shortcut, name, districtId, latitude, longitude) VALUES (@id, @shortcut, @name, @districtId, @lat, @lng)";
        private const string DELETE_CMD = "DELETE FROM Venue WHERE venueId = @id";
        private const string GETALL_CMD = "SELECT * FROM Venue";
        private const string GETBYID_CMD = "SELECT * FROM Venue WHERE venueId = @id";
        private const string UPDATE_CMD = "UPDATE Venue SET shortcut=@shortcut, name=@name, districtId=@districtId, latitude=@lat, longitude=@lng WHERE venueId=@id";

        private IDatabase db;

        private Venue readOne(DbDataReader reader) {
            string id = (string)reader["venueId"];
            string shortcut = (string)reader["shortcut"];
            string name = (string)reader["name"];
            uint districtId = (uint)reader["districtId"];
            double lat = (double)reader["latitude"];
            double lng = (double)reader["longitude"];
            return new Venue(id, name, shortcut, districtId, lat, lng);
        }

        public VenueDao(IDatabase db) {
            this.db = db;
        }

        public Venue CreateVenue(string id, string name, string shortcut, uint districtId, double latitude, double longitude) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.String, id);
            db.DefineParameter(cmd, "@shortcut", System.Data.DbType.String, shortcut);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, name);
            db.DefineParameter(cmd, "@districtId", System.Data.DbType.UInt32, districtId);
            db.DefineParameter(cmd, "@lat", System.Data.DbType.Double, latitude);
            db.DefineParameter(cmd, "@lng", System.Data.DbType.Double, longitude);
            db.ExecuteNonQuery(cmd);
            return new Venue(id, name, shortcut, districtId, latitude, longitude);
        }

        public void DeleteAllVenues() {
            DbCommand cmd = db.CreateCommand(DELETEALL_CMD);
            db.ExecuteNonQuery(cmd);
        }

        public void DeleteVenue(Venue ven) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.String, ven.Id);
            cmd.ExecuteNonQuery();
        }

        public List<Venue> GetAllVenues() {
            List<Venue> venues = new List<Venue>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    venues.Add(readOne(reader));
                }
            }
            return venues;
        }

        public Venue GetVenueById(string id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.String, id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }

        public void UpdateVenue(Venue ven) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.String, ven.Id);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, ven.Name);
            db.DefineParameter(cmd, "@shortcut", System.Data.DbType.String, ven.Shortcut);
            db.DefineParameter(cmd, "@districtId", System.Data.DbType.UInt32, ven.DistrictId);
            db.DefineParameter(cmd, "@lat", System.Data.DbType.Double, ven.Latitude);
            db.DefineParameter(cmd, "@lng", System.Data.DbType.Double, ven.Longitude);
            db.ExecuteNonQuery(cmd);
        }
    }
}
