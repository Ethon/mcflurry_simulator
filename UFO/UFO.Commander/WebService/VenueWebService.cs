﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.WebService
{
    /*
    GET api/Venue/GetVenueById/{id}	
GET api/Venue/GetAllVenues	
POST api/Venue/UpdateVenue	
POST 	
GET 
*/
    class VenueWebService : IVenueService
    {
        private RestClient client;
        private List<IVenueListener> listeners;

        public VenueWebService(RestClient client)
        {
            this.client = client;
        }

        public void AddListener(IVenueListener listener)
        {
            listeners.Add(listener);
        }

        public Venue CreateVenue(string name, string shortcut, double latitude, double longitude)
        {
            var request = new RestRequest("api/Venue/CreateVenue?name={name}&shortcut={shortcut}&latitude={latitude}&longitude={longitude}", Method.GET);
            request.AddUrlSegment("name", name);
            request.AddUrlSegment("shortcut", shortcut);
            request.AddUrlSegment("latitude", latitude.ToString());
            request.AddUrlSegment("longitude", longitude.ToString());
            Venue venue = client.Execute<Venue>(request).Data;
            if(venue != null)
            {
                foreach (var listener in listeners)
                {
                    listener.OnVenueCreation(venue);
                }
            }
            return venue;
        }

        public void DeleteVenue(Venue venue)
        {
            var request = new RestRequest("api/Venue/DeleteVenue", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(venue);
            client.Execute(request);
        }

        public List<Venue> GetAllVenues()
        {
            return new List<Venue>();
        }

        public Venue GetVenueById(uint id)
        {
            return new Venue();
        }

        public void RemoveListener(IVenueListener listener)
        {
            listeners.Remove(listener);
        }

        public void UpdateVenue(Venue venue)
        {
            throw new NotImplementedException();
        }
    }
}
