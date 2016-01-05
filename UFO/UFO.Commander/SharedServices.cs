using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Commander.WebService;
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

        public static void Init(RestClient client)
        {
            if (Instance != null)
            {
                return;
            }

            Instance = new SharedServices();
            Instance.CategoryService = new CategoryWebService(client);
            Instance.ArtistService = new ArtistWebService(client);
            Instance.CountryService = new CountryWebService(client);
            Instance.VenueService = new VenueWebService(client);
            Instance.UserService = new UserWebService(client);
            Instance.PerformanceService = new PerformanceWebService(client);
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
