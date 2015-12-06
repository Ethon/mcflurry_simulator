using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class ArtistViewModel : INotifyPropertyChanged {
        private IArtistService artistService;
        private ICategoryService categoryService;
        private Artist artist;
        private Category category;
        public event PropertyChangedEventHandler PropertyChanged;

        public ArtistViewModel(IArtistService artistService, ICategoryService categoryService, Artist artist) {
            this.artistService = artistService;
            this.categoryService = categoryService;
            this.artist = artist;
        }

        public uint Id {
            get {
                return artist.Id;
            }
        }

        public string Name {
            get {
                return artist.Name;
            }
            set {
                if(value != artist.Name) {
                    artist.Name = value;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public string Email {
            get {
                return artist.Email;
            }
            set {
                if(value != artist.Email) {
                    artist.Email = value;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public Category Category {
            get {
                if(category == null) {
                    category = categoryService.GetCategoryById(artist.Id);
                }
                return category;
            }
            set {
                if(category != value) {
                    category = value;
                    artist.CategoryId = category.Id;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Category)));
                }
            }
        }

        public void Update() {
            artistService.UpdateArtist(artist);
        }

    }
}
