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

        public static IVenueService CreateVenueService(IDatabase db) {
            return new VenueService(db);
        }

        public static IUserService CreateUserService(IDatabase db) {
            return new UserService(db);
        }

        public static ICategoryService CreateCategoryService(IDatabase db) {
            return new CategoryService(db);
        }

        public static IPerformanceService CreatePerformanceService(IDatabase db) {
            return new PerformanceService(db);
        }
    }
}
