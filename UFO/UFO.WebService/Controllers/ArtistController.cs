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

        [HttpGet]
        public Artist GetArtistById(uint id)
        {
            return artistService.GetArtistById(id);
        }

        [HttpGet]
        public Artist GetArtistByName(string name)
        {
            return artistService.GetArtistByName(name);
        }

        [HttpGet]
        public Artist[] GetAllArtists()
        {
            return artistService.GetAllArtists().ToArray();
        }

        [HttpPost]
        public void UpdateArtist(Artist artist)
        {
            artistService.UpdateArtist(artist);
        }

        [HttpPost]
        public void DeleteArtist(Artist artist)
        {
            artistService.DeleteArtist(artist);
        }

        [HttpPost]
        public Artist CreateArtist(Artist artist)
        {
            return artistService.CreateArtist(artist.Name, artist.Email, new Category(artist.CategoryId, null, null),
                new Country(artist.CountryId, null, null), artist.PicturePath, artist.VideoPath);
        }
    }
}
