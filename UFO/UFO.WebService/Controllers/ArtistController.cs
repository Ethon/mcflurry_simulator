using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.WebService.Controllers
{
    public class ArtistController : ApiController
    {
        private IArtistService artistService;

        public ArtistController()
        {
            artistService = ServiceFactory.CreateArtistService(DatabaseConnection.GetDatabase());
        }

        public Artist GetArtistById(uint id)
        {
            return artistService.GetArtistById(id);
        }

        public Artist[] GetAllArtists()
        {
            return artistService.GetAllArtists().ToArray();
        }

        public void UpdateArtist(Artist artist)
        {
            artistService.UpdateArtist(artist);
        }

        public void DeleteArtist(Artist artist)
        {
            artistService.DeleteArtist(artist);
        }

        public Artist CreateArtist(string name, string email, Category category, Country country, string picturePath, string videoPath)
        {
            return artistService.CreateArtist(name, email, category, country, picturePath, videoPath);
        }
    }
}
