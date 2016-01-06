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
            Performance param = new Performance(0, date, artist.Id, venue.Id);
            var request = new RestRequest("api/Performance/CreatePerformance", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(param);
            return client.Execute<Performance>(request).Data;
        }

        public void DeletePerformance(Performance performance)
        {
            var request = new RestRequest("api/Performance/DeletePerformance", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(performance);
            client.Execute(request);
        }

        public List<Performance> GetAllPerformances()
        {
            var request = new RestRequest("api/Performance/GetAllPerformances", Method.GET);
            return client.Execute<List<Performance>>(request).Data;
        }

        public Performance GetPerformanceById(uint id)
        {
            var request = new RestRequest("api/Performance/GetPerformanceById/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return client.Execute<Performance>(request).Data;
        }

        public List<Performance> GetPerformancesForDay(DateTime date)
        {
            var request = new RestRequest("api/Performance/GetPerformancesForDay?date={date}", Method.GET);
            string dateString = string.Format("{0}-{1}-{2}", date.Year, date.Month, date.Day);
            string escapedDateString = System.Uri.EscapeDataString(dateString);
            request.AddUrlSegment("date", escapedDateString);
            return client.Execute<List<Performance>>(request).Data;
        }

        public void UpdatePerformance(Performance performance)
        {
            var request = new RestRequest("api/Performance/UpdatePerformance", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(performance);
            client.Execute(request);
        }
    }
}
