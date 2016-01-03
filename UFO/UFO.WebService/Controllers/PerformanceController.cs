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

        [HttpGet]
        public Performance GetPerformanceById(uint id) {
            return ps.GetPerformanceById(id);
        }

        [HttpGet]
        public Performance[] GetAllPerformances() {
            return ps.GetAllPerformances().ToArray();
        }

        [HttpGet]
        public void GetPerformancesForDay(DateTime date) {
            ps.GetPerformancesForDay(date);
        }

	[HttpPost]
        public void DeletePerformance(Performance performance) {
            ps.DeletePerformance(performance);
        }

        [HttpPost]
        public void UpdatePerformance(Performance performance) {
            ps.UpdatePerformance(performance);
        }

        [HttpGet]
        public Performance CreatePerformance(DateTime date, Artist artist, Venue venue) {
            return ps.CreatePerformance(date, artist, venue);
        }

    }
}