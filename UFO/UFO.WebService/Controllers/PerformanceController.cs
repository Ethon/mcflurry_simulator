using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers {
    public class PerformanceController : ApiController {
        private IPerformanceService ps;

        public PerformanceController() {
            ps = ServiceFactory.CreatePerformanceService(DatabaseConnection.GetDatabase());
        }

        public Performance GetPerformanceById(uint id) {
            return ps.GetPerformanceById(id);
        }

        public Performance[] GetAllPerformances() {
            return ps.GetAllPerformances().ToArray();
        }

        public void GetPerformancesForDay(DateTime date) {
            ps.GetPerformancesForDay(date);
        }

        public void DeleteArtist(Performance performance) {
            ps.DeletePerformance(performance);
        }

        public void UpdatePerformance(Performance performance) {
            ps.UpdatePerformance(performance);
        }
  
        public Performance CreatePerformance(DateTime date, Artist artist, Venue venue) {
            return ps.CreatePerformance(date, artist, venue);
        }

    }
}