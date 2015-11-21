using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Server {
    public class VenueService {
        private const double LAT_MIN = -90.0;
        private const double LAT_MAX = +90.0;
        private const double LNG_MIN = -180.0;
        private const double LNG_MAX = +180.0;

        private static Regex shortcutRegex = new Regex("^\\p{L}+\\d+$");

        private IVenueDao vdao;
        private IPerformanceDao pdao;

        private static bool IsValidName(string name) {
            return true;
        }

        private static bool IsValidShortcut(string shortcut) {
            return shortcutRegex.IsMatch(shortcut);
        }

        private static bool IsValidLatitude(double lat) {
            return lat >= LAT_MIN && lat <= LAT_MAX;
        }

        private static bool IsValidLongitude(double lat) {
            return lat >= LNG_MIN && lat <= LNG_MAX;
        }

        private bool IsUsedVenue(Venue venue) {
            return pdao.CountOfPerformancesAtVenue(venue) > 0;
        }

        public VenueService(IDatabase db) {
            if (db is MYSQLDatabase) {
                vdao = new VenueDao(db);
                pdao = new PerformanceDao(db);
            } else {
                throw new NotSupportedException("Database not supported");
            }
        }

        public Venue CreateVenue(string name, string shortcut, double latitude, double longitude) {
            if(!IsValidName(name)) {
                throw new DataValidationException("Can't create venue with invalid name '" + name + "'");
            } else if(!IsValidShortcut(shortcut)) {
                throw new DataValidationException("Can't create venue with invalid shortcut '" + shortcut + "'");
            } else if(!IsValidLatitude(latitude)) {
                throw new DataValidationException("Can't create venue with invalid latitude '" + latitude + "'");
            } else if (!IsValidLongitude(longitude)) {
                throw new DataValidationException("Can't create venue with invalid longitude '" + longitude + "'");
            }

            return vdao.CreateVenue(name, shortcut, latitude, longitude);
        }

        public List<Venue> GetAllVenues() {
            return vdao.GetAllVenues();
        }

        public Venue GetVenueById(uint id) {
            return vdao.GetVenueById(id);
        }

        public void UpdateVenue(Venue venue) {
            if (!IsValidName(venue.Name)) {
                throw new DataValidationException("Can't update venue to invalid name '" + venue.Name + "'");
            } else if (!IsValidShortcut(venue.Shortcut)) {
                throw new DataValidationException("Can't update venue to invalid shortcut '" + venue.Shortcut + "'");
            } else if (!IsValidLatitude(venue.Latitude)) {
                throw new DataValidationException("Can't update venue to invalid latitude '" + venue.Latitude + "'");
            } else if (!IsValidLongitude(venue.Longitude)) {
                throw new DataValidationException("Can't update venue to invalid longitude '" + venue.Longitude + "'");
            }

            if (!vdao.UpdateVenue(venue)) {
                throw new DatabaseException("DatabaseError: Can`t update venue " + venue);
            };
        }

        public void DeleteVenue(Venue venue) {
            if(IsUsedVenue(venue)) {
                throw new DataValidationException("Can't delete used venue '" + venue.Name + "'");
            }

            if (!vdao.DeleteVenue(venue)) {
                throw new DatabaseException("DatabaseError: Can`t delete venue " + venue);
            };
        }
    }
}
