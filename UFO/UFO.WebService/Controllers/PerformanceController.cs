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
        public Performance[] GetPerformancesForDay(string date) {
            string[] parts = date.Split('-');
            return ps.GetPerformancesForDay(new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]))).ToArray();
        }

	    [HttpPost]
        public void DeletePerformance(Performance performance) {
            ps.DeletePerformance(performance);
        }

        [HttpPost]
        public void UpdatePerformance(Performance performance) {
            ps.UpdatePerformance(performance);
        }

        [HttpPost]
        public Performance CreatePerformance(Performance param) {
            return ps.CreatePerformance(param.Date, new Artist { Id = param.ArtistId }, new Venue { Id = param.VenueId });
        }
    }
}