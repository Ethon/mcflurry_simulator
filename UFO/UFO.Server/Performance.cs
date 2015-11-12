using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server.Data {
    public class Performance {
        public Performance(uint id, DateTime date, uint artistId, uint venueId) {
            Id = id;
            Date = date;
            ArtistId = artistId;
            VenueId = venueId;
        }

        public uint Id {
            get; private set;
        }

        public DateTime Date {
            get; set;
        }

        public uint ArtistId {
            get; set;
        }

        public uint VenueId {
            get; set;
        }

        public override string ToString() {
            return String.Format("Performance(id={0}, date={1}, artistId={2}, venueId={3}) ", Id, Date, ArtistId, VenueId);
        }

        public override bool Equals(object obj) {
            Performance per = obj as Performance;
            if (per == null) {
                return false;
            }
            return Id.Equals(per.Id) && Date.Equals(per.Date) && ArtistId.Equals(per.ArtistId) && VenueId.Equals(per.VenueId);
        }

        public override int GetHashCode() {
            return (int)Id;
        }
    }

    public interface IPerformanceDao {
        List<Performance> GetAllPerformances();
        Performance GetPerformanceById(uint id);
        bool UpdatePerformance(Performance performance);
        bool DeletePerformance(Performance performance);
        Performance CreatePerformance(DateTime date, uint artistId, uint venueId);
        void DeleteAllPerformances();
        uint CountOfPerformancesAtVenue(Venue venue);
        uint CountOfPerformancesOfArtist(Artist artist);
        Performance GetPerformanceByVenueAndDate(uint venueId, DateTime date);
    }

    public class PerformanceDao : IPerformanceDao {
        private const string CREATE_CMD = "INSERT INTO Performance(date, artistId, venueId) VALUES (@date, @artistId, @venueId)";
        private const string DELETE_CMD = "DELETE FROM Performance WHERE performanceId = @id";
        private const string GETALL_CMD = "SELECT * FROM Performance";
        private const string GETBYID_CMD = "SELECT * FROM Performance WHERE performanceId = @id";
        private const string UPDATE_CMD = "UPDATE Performance SET date=@date, artistId=@artistId, venueId=@venueId WHERE performanceId=@id";
        private const string COUNTVENUES_CMD = "SELECT COUNT(*) AS count FROM Performance WHERE venueId=@venueId";
        private const string COUNTARTISTS_CMD = "SELECT COUNT(*) AS count FROM Performance WHERE artistId=@artistId";
        private const string GETBYVENUEDATE_CMD = "SELECT * FROM Performance WHERE date=@date AND venueId=@venueId";

        private IDatabase db;

        private Performance readOne(DbDataReader reader) {
            uint id = (uint)reader["performanceId"];
            DateTime date = db.ConvertDateTimeFromDbFormat(reader["date"]);
            uint artistId = (uint)reader["artistId"];
            uint venueId = (uint)reader["venueId"];
            return new Performance(id, date, artistId, venueId);
        }

        public PerformanceDao(IDatabase db) {
            this.db = db;
        }

        public Performance CreatePerformance(DateTime date, uint artistId, uint venueId) {
            DbCommand cmd = db.CreateCommand(CREATE_CMD);
            db.DefineParameter(cmd, "@date", System.Data.DbType.DateTime, db.ConvertDateTimeToDbFormat(date));
            db.DefineParameter(cmd, "@artistId", System.Data.DbType.UInt32, artistId);
            db.DefineParameter(cmd, "@venueId", System.Data.DbType.UInt32, venueId);
            uint id = (uint)db.ExecuteNonQuery(cmd);
            return new Performance(id, date, artistId, venueId);
        }

        public void DeleteAllPerformances() {
            db.TruncateTable("Performance");
        }

        public bool DeletePerformance(Performance performance) {
            DbCommand cmd = db.CreateCommand(DELETE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, performance.Id);
            return cmd.ExecuteNonQuery() >= 1;
        }

        public List<Performance> GetAllPerformances() {
            List<Performance> performances = new List<Performance>();
            DbCommand cmd = db.CreateCommand(GETALL_CMD);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                while (reader.Read()) {
                    performances.Add(readOne(reader));
                }
            }
            return performances;
        }

        public Performance GetPerformanceById(uint id) {
            DbCommand cmd = db.CreateCommand(GETBYID_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }

        public bool UpdatePerformance(Performance performance) {
            DbCommand cmd = db.CreateCommand(UPDATE_CMD);
            db.DefineParameter(cmd, "@id", System.Data.DbType.UInt32, performance.Id);
            db.DefineParameter(cmd, "@date", System.Data.DbType.DateTime, db.ConvertDateTimeToDbFormat(performance.Date));
            db.DefineParameter(cmd, "@artistId", System.Data.DbType.UInt32, performance.ArtistId);
            db.DefineParameter(cmd, "@venueId", System.Data.DbType.UInt32, performance.VenueId);
            return db.ExecuteNonQuery(cmd) >= 1;
        }

        public uint CountOfPerformancesAtVenue(Venue venue) {
            DbCommand cmd = db.CreateCommand(COUNTVENUES_CMD);
            db.DefineParameter(cmd, "@venueId", System.Data.DbType.UInt32, venue.Id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                reader.Read();
                return (uint)((long)reader["count"]);
            }
        }

        public uint CountOfPerformancesOfArtist(Artist artist) {
            DbCommand cmd = db.CreateCommand(COUNTARTISTS_CMD);
            db.DefineParameter(cmd, "@artistId", System.Data.DbType.UInt32, artist.Id);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                reader.Read();
                return (uint)((long)reader["count"]);
            }
        }

        public Performance GetPerformanceByVenueAndDate(uint venueId, DateTime date) {
            DbCommand cmd = db.CreateCommand(GETBYVENUEDATE_CMD);
            db.DefineParameter(cmd, "@date", System.Data.DbType.DateTime, date);
            db.DefineParameter(cmd, "@venueId", System.Data.DbType.UInt32, venueId);
            using (DbDataReader reader = cmd.ExecuteReader()) {
                if (reader.Read()) {
                    return readOne(reader);
                } else {
                    return null;
                }
            }
        }
    }
}
