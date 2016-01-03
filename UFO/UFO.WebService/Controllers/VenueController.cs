using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers {
    public class VenueController : ApiController {
        private IVenueService vs;

        public VenueController() {
            IDatabase db = DatabaseConnection.GetDatabase();
            vs = ServiceFactory.CreateVenueService(db);
        }

        public Venue GetVenueById(uint id) {
            return vs.GetVenueById(id);
        }

        public Venue[] GetAllVenues() {
            
            return vs.GetAllVenues().ToArray();
        }
        public void UpdateVenue(Venue venue) {
            vs.UpdateVenue(venue);
        }

        public void DeleteVenue(Venue venue) {
            vs.DeleteVenue(venue);
        }

        public Venue CreateVenue(String name, String shortcut,double latitude, double longitude) {
            return vs.CreateVenue(name, shortcut, latitude, longitude);
        }
    }
}