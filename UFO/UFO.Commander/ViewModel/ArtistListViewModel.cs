using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;

namespace UFO.Commander.ViewModel {
    public class ArtistListViewModel {
        private IArtistService artistService;
        private ICategoryService categoryService;

        public ArtistListViewModel(IArtistService artistService, ICategoryService categoryService) {
            this.artistService = artistService;
            this.categoryService = categoryService;
            Artists = new ObservableCollection<ArtistViewModel>();
            UpdateArtists();
        }

        public ObservableCollection<ArtistViewModel> Artists {
            get; set;
        }

        public void UpdateArtists() {
            Artists.Clear();
            foreach (var artist in artistService.GetAllArtists()) {
                Artists.Add(new ArtistViewModel(artistService, categoryService, artist));
            }
        }
    }
}
