using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Server {
    public class ServiceFactory {
        public static IArtistService CreateArtistService(IDatabase db) {
            return new ArtistService(db);
        }
    }
}
