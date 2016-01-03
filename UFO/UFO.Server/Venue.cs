using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Venue {
        public Venue(uint id, string name, string shortcut, double lat, double lng) {
            Id = id;
            Name = name;
            Shortcut = shortcut;
            Latitude = lat;
            Longitude = lng;
        }

        public uint Id {
            get; private set;
        }

        public string Name {
            get; set;
        }

        public string Shortcut {
            get; set;
        }

        public double Latitude {
            get; set;
        }

        public double Longitude {
            get; set;
        }

        public override string ToString() {
            return String.Format("Venue(id={0}, name='{1}', shortcut='{2}',  latitude={3}, longitude={4})",
                Id, Name, Shortcut,  Latitude, Longitude);
        }

        public override bool Equals(object obj) {
            Venue ven = obj as Venue;
            if (ven == null) {
                return false;
            }
            return  Id.Equals(ven.Id) &&
                    Name.Equals(ven.Name) &&
                    Shortcut.Equals(ven.Shortcut) &&
                    Latitude.Equals(ven.Latitude) &&
                    Longitude.Equals(ven.Longitude);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface IVenueDao {
        void DeleteAllVenues();
        List<Venue> GetAllVenues();
        Venue GetVenueById(uint id);
        bool UpdateVenue(Venue ven);
        bool DeleteVenue(Venue ven);
        Venue CreateVenue(string name, string shortcut, double latitude, double longitude);
    }

    public class VenueDao : IVenueDao {
        private const string CREATE_CMD = "INSERT INTO Venue(shortcut, name, latitude, longitude) VALUES (@shortcut, @name,@lat, @lng)";
        private const string DELETE_CMD = "DELETE FROM Venue WHERE venueId = @id";
        private const string GETALL_CMD = "SELECT * FROM Venue";
        private const string GETBYID_CMD = "SELECT * FROM Venue WHERE venueId = @id";
        private const string UPDATE_CMD = "UPDATE Venue SET shortcut=@shortcut, name=@name,latitude=@lat, longitude=@lng WHERE venueId=@id";

        private IDatabase db;

        private Venue readOne(IDataReader reader) {
            uint id = (uint)reader["venueId"];
            string shortcut = (string)reader["shortcut"];
            string name = (string)reader["name"];
            double lat = (double)reader["latitude"];
            double lng = (double)reader["longitude"];
            return new Venue(id, name, shortcut, lat, lng);
        }

        public VenueDao(IDatabase db) {
            this.db = db;
        }

        public Venue CreateVenue(string name, string shortcut, double latitude, double longitude) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@shortcut", System.Data.DbType.String, shortcut);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, name);
            db.DefineParameter(cmd, "@lat", System.Data.DbType.Double, latitude);
            db.DefineParameter(cmd, "@lng", System.Data.DbType.Double, longitude);
            uint id = (uint)db.ExecuteNonQuery(cmd);
            return new Venue(id, name, shortcut, latitude, longitude);
        }

        public void DeleteAllVenues() {
            db.TruncateTable("Venue");
        }

        public bool DeleteVenue(Venue ven) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, ven.Id);
            return cmd.ExecuteNonQuery() >= 1;
        }

        public List<Venue> GetAllVenues() {
            List<Venue> venues = new List<Venue>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            db.doSynchronized(() => {
                using (IDataReader reader = db.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        venues.Add(readOne(reader));
                    }
                }
            });
            return venues;
        }

        public Venue GetVenueById(uint id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, id);
            Venue venue = null;
            db.doSynchronized(() => {
                using (IDataReader reader = db.ExecuteReader(cmd)) {
                    if (reader.Read()) {
                        venue = readOne(reader);
                    }
                }
            });
            return venue;
        }

        public bool UpdateVenue(Venue ven) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, ven.Id);
            db.DefineParameter(cmd, "@name", System.Data.DbType.String, ven.Name);
            db.DefineParameter(cmd, "@shortcut", System.Data.DbType.String, ven.Shortcut);
            db.DefineParameter(cmd, "@lat", System.Data.DbType.Double, ven.Latitude);
            db.DefineParameter(cmd, "@lng", System.Data.DbType.Double, ven.Longitude);
            return db.ExecuteNonQuery(cmd) >= 1;
        }
    }
}
