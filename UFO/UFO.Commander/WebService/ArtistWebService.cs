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

        public ArtistWebService(RestClient client)
        {
            this.client = client;
        }

        public void AddListener(IArtistListener listener)
        {
        }

        public Artist CreateArtist(string name, string email, Category category, Country country, string picturePath, string videoPath)
        {
            return new Artist();
        }

        public void DeleteArtist(Artist artist)
        {
            throw new NotImplementedException();
        }

        public List<Artist> GetAllArtists()
        {
            return new List<Artist>();
        }

        public Artist GetArtistById(uint id)
        {
            return new Artist();
        }

        public void RemoveListener(IArtistListener listener)
        {
        }

        public void UpdateArtist(Artist artist)
        {
            throw new NotImplementedException();
        }
    }
}
