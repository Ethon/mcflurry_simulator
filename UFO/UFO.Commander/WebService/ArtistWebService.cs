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
    class ArtistWebService : IArtistService
    {
        private RestClient client;
        private List<IArtistListener> listeners;

        public ArtistWebService(RestClient client)
        {
            this.client = client;
            this.listeners = new List<IArtistListener>();
        }

        public void AddListener(IArtistListener listener)
        {
            listeners.Add(listener);
        }

        public Artist CreateArtist(string name, string email, Category category, Country country, string picturePath, string videoPath)
        {
            Artist param = new Artist(0, name, email, category.Id, country.Id, picturePath, videoPath);
            var request = new RestRequest("api/Artist/CreateArtist", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(param);
            Artist artist = client.Execute<Artist>(request).Data;
            if (artist != null)
            {
                foreach (var listener in listeners)
                {
                    listener.OnArtistCreation(artist);
                }
            }
            return artist;
        }

        public void DeleteArtist(Artist artist)
        {
            var request = new RestRequest("api/Artist/DeleteArtist", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(artist);
            client.Execute(request);
            foreach (var listener in listeners)
            {
                listener.OnArtistDeletion(artist);
            }
        }

        public List<Artist> GetAllArtists()
        {
            var request = new RestRequest("api/Artist/GetAllArtists", Method.GET);
            return client.Execute<List<Artist>>(request).Data;
        }

        public Artist GetArtistById(uint id)
        {
            var request = new RestRequest("api/Artist/GetArtistById/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            return client.Execute<Artist>(request).Data;
        }

        public void RemoveListener(IArtistListener listener)
        {
            listeners.Remove(listener);
        }

        public void UpdateArtist(Artist artist)
        {
            var request = new RestRequest("api/Artist/UpdateArtist", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(artist);
            client.Execute(request);
            foreach (var listener in listeners)
            {
                listener.OnArtistUpdate(artist);
            }
        }
    }
}
