using Microsoft.Maps.MapControl.WPF;
using UFO.Commander.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class VenueManagementViewModel :INotifyPropertyChanged {
        private IVenueService venueService;
        private VenueViewModel currentVenue;
        public event PropertyChangedEventHandler PropertyChanged;

        public string NameInput { get; set; }
        public string ShortCutInput { get; set; }
        public double LatitudeInput { get; set; }
        public double LongitudeInput { get; set; }

        private ICommand createCommand;

        public ObservableCollection<VenueViewModel> Venues { get; set; }

        public VenueManagementViewModel(IVenueService venueService) {
            this.venueService = venueService;
            NameInput = "";
            ShortCutInput = "";
            this.Venues = new ObservableCollection<VenueViewModel>();
            UpdateVenues();
        }

        public void UpdateVenues() {
            CurrentVenue = null;
            Venues.Clear();
            Task.Run(() => {
                List<Venue> venues = venueService.GetAllVenues();
                foreach (var venue in venues) {
                    VenueViewModel vm = new VenueViewModel(venueService, venue);
                    PlatformService.Instance.RunByUiThread(() => {
                        Venues.Add(vm);
                    });
                }
            });
        }

        public VenueViewModel CurrentVenue {
            get {
                return this.currentVenue;
            }
            set {
                if (this.currentVenue != value) {
                    this.currentVenue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentVenue)));
                }
            }
        }

        public ICommand CreateCommand {
            get {
                if (createCommand == null) {
                    createCommand = new RelayCommand((param) => {
                    Venue venue;
                    try {
                        venue = venueService.CreateVenue(NameInput, ShortCutInput,LatitudeInput,LongitudeInput);
                    } catch (DataValidationException ex) {
                        PlatformService.Instance.ShowErrorMessage(ex.Message, "Error creating venue");
                        return;
                    }
                    VenueViewModel newVenue = new VenueViewModel(venueService, venue);
                    Venues.Add(newVenue);
                        PlatformService.Instance.ShowInformationMessage("Created venue '" + venue.Name + "'!","Information");
                        NameInput = "";
                        ShortCutInput = "";
                        LongitudeInput = 0;
                        LatitudeInput = 0;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameInput)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortCutInput)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LatitudeInput)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongitudeInput)));
                        currentVenue = newVenue;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentVenue)));
                    });
                }
                return createCommand;
            }
        }
    }
}
