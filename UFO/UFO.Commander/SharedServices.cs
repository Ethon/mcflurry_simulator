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
            Instance.CountryService = ServiceFactory.CreateCountryService(db);
            Instance.VenueService = ServiceFactory.CreateVenueService(db);
            Instance.UserService = ServiceFactory.CreateUserService(db);
            Instance.PerformanceService = ServiceFactory.CreatePerformanceService(db);
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

        public ICountryService CountryService {
            get; private set;
        }

        public IVenueService VenueService {
            get; private set;
        }

        public IUserService UserService {
            get; private set;
        }

        public IPerformanceService PerformanceService {
            get; private set;
        }
    }
}
