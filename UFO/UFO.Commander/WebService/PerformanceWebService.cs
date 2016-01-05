using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.WebService
{
    class PerformanceWebService : IPerformanceService
    {
        private RestClient client;

        public PerformanceWebService(RestClient client)
        {
            this.client = client;
        }

        public Performance CreatePerformance(DateTime date, Artist artist, Venue venue)
        {
            return new Performance();
        }

        public void DeletePerformance(Performance performance)
        {
            throw new NotImplementedException();
        }

        public List<Performance> GetAllPerformances()
        {
            return new List<Performance>();
        }

        public Performance GetPerformanceById(uint id)
        {
            return new Performance();
        }

        public List<Performance> GetPerformancesForDay(DateTime date)
        {
            return new List<Performance>();
        }

        public void UpdatePerformance(Performance performance)
        {
            throw new NotImplementedException();
        }
    }
}
