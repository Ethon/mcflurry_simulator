using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UFO.Commander.ViewModel;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class ArtistManagementViewModel : INotifyPropertyChanged {

        private IArtistService artistService;
        private ICategoryService categoryService;
        private ICountryService countryService;

        private string picturePathInput;
        private string videoPathInput;

        private ICommand createCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ArtistManagementViewModel(IArtistService artistService, ICategoryService categoryService, ICountryService countryService) {

            this.artistService = artistService;
            this.categoryService = categoryService;
            this.countryService = countryService;

            Artists = new ObservableCollection<ArtistViewModel>();
            Categories = new ObservableCollection<Category>();
            Countries = new ObservableCollection<Country>();

            NameInput = "";
            EmailInput = "";
            if(Categories.Count > 0) {
                CategoryInput = Categories[0];
            }
            if(Countries.Count > 0) {
                CountryInput = Countries[0];
            }

            UpdateCategories();
            UpdateArtists();
            UpdateCountries();
        }

        public string NameInput { get; set; }
        public string EmailInput { get; set; }
        public Category CategoryInput { get; set; }
        public Country CountryInput { get; set; }

        public string PicturePathInput {
            get {
                return picturePathInput;
            }
            set {
                if (value != picturePathInput) {
                    picturePathInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PicturePathInput)));
                }
            }
        }

        public string VideoPathInput {
            get {
                return videoPathInput;
            }
            set {
                if (value != videoPathInput) {
                    videoPathInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VideoPathInput)));
                }
            }
        }

        public ICommand CreateCommand {
            get {
                if(createCommand == null) {
                    createCommand = new RelayCommand((param) => {
                        Artist artist;
                        try { 
                            artist = artistService.CreateArtist(NameInput, EmailInput, CategoryInput, CountryInput, PicturePathInput, VideoPathInput);
                        } catch(DataValidationException ex) {
                            PlatformService.Instance.ShowErrorMessage(ex.Message, "Error creating artist");
                            return;
                        }
                        Artists.Add(new ArtistViewModel(artistService, categoryService, countryService, artist));
                  });
                }
                return createCommand;
            }
        }

        public ObservableCollection<ArtistViewModel> Artists {
            get; private set;
        }

        public ObservableCollection<Category> Categories {
            get; private set;
        }

        public ObservableCollection<Country> Countries {
            get; private set;
        }

        public void UpdateArtists() {
            Artists.Clear();
            foreach (var artist in artistService.GetAllArtists()) {
                Artists.Add(new ArtistViewModel(artistService, categoryService, countryService, artist));
            }
        }

        public void UpdateCategories() {
            Categories.Clear();
            foreach (var category in categoryService.GetAllCategories()) {
                Categories.Add(category);
            }
        }

        public void UpdateCountries() {
            Countries.Clear();
            foreach (var country in countryService.GetAllCountries()) {
                Countries.Add(country);
            }
        }
    }
}
