using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;

namespace UFO.Commander {
    class SharedServices {

        public static void Init(IDatabase db) {
            if(Instance != null) {
                return;
            }

            Instance = new SharedServices();
            Instance.CategoryService = ServiceFactory.CreateCategoryService(db);
            Instance.ArtistService = ServiceFactory.CreateArtistService(db);
        }

        public static SharedServices Instance {
            get; private set;
        }

        public ICategoryService CategoryService {
            get; private set;
        }

        public IArtistService ArtistService {
            get; private set;
        }
    }
}
