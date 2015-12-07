using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class ArtistViewModel : INotifyPropertyChanged {
        private IArtistService artistService;
        private ICategoryService categoryService;
        private ICountryService countryService;

        private Artist artist;
        private Category category;
        private Country country;

        private ICommand deleteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ArtistViewModel(IArtistService artistService, ICategoryService categoryService,
            ICountryService countryService, Artist artist) {
            this.artistService = artistService;
            this.categoryService = categoryService;
            this.countryService = countryService;
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
                    category = categoryService.GetCategoryById(artist.CategoryId);
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

        public Country Country {
            get {
                if(country == null) {
                    country = countryService.GetCountryById(artist.CountryId);
                }
                return country;
            }
            set {
                if(country != value) {
                    country = value;
                    artist.CountryId = country.Id;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Country)));
                }
            }
        }

        public string PicturePath {
            get {
                return artist.PicturePath;
            }
            set {
                if(artist.PicturePath != value) {
                    artist.PicturePath = value;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PicturePath)));
                }
            }
        }

        public string VideoPath {
            get {
                return artist.VideoPath;
            }
            set {
                if(value != artist.VideoPath) {
                    artist.VideoPath = value;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VideoPath)));
                }
            }
        }

        public ICommand DeleteCommand {
            get {
                if(deleteCommand == null) {
                    deleteCommand = new RelayCommand((param) => {
                        if(PlatformService.Instance.WarnAndAskForConfirmation(
                                "Do you really want to delete the artist " + Name + "?", "Confirm deletion")) {
                            Delete();
                            ArtistManagementViewModel amvm = param as ArtistManagementViewModel;
                            amvm.UpdateArtists();
                        }
                    });
                }
                return deleteCommand;
            }
        }

        public void Update() {
            artistService.UpdateArtist(artist);
        }

        public void Delete() {
            artistService.DeleteArtist(artist);
        }
    }
}
