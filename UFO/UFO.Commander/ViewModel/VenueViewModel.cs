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
            foreach (var venue in venueService.GetAllVenues()) {
                Venues.Add(new VenueViewModel(venueService, venue));
            }
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
    public class VenueViewModel : INotifyPropertyChanged {

        private IVenueService venueService;
        public Venue venue;
        private ICommand deleteCommand;


        public event PropertyChangedEventHandler PropertyChanged;

        public VenueViewModel(IVenueService venueService, Venue venue) {
            this.venueService = venueService;
            this.venue = venue;
        }

        public uint Id {
            get { return venue.Id; }
        }

        public string ShortCut {
            get { return venue.Shortcut; }
            set {
                if (venue.Shortcut != value) {
                    venue.Shortcut = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortCut)));
                }
            }
        }
        public string Name {
            get { return venue.Name; }
            set {
                if (venue.Name != value) {
                    venue.Name = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public Location Location
        {
            get
            {
                return new Location(venue.Latitude, venue.Longitude);
            }
            set
            {
                Longitude = value.Longitude;
                Latitude = value.Latitude;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
            }
        }

        public double Longitude {
            get { return venue.Longitude; }
            set {
                if (venue.Longitude != value) {
                    venue.Longitude = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
                }
            }
        }
        public double Latitude {
            get { return venue.Latitude; }
            set {
                if (venue.Latitude != value) {
                    venue.Latitude = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude)));
                }
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null) {
                    deleteCommand = new RelayCommand((param) => {
                        if (PlatformService.Instance.WarnAndAskForConfirmation(
                                "Do you really want to delete the venue '" + Name + "'?", "Confirm deletion")) {
                    Delete();
                            VenueManagementViewModel VmVm = param as VenueManagementViewModel;
                            VmVm.UpdateVenues();
                        }
                    });
                }
                return deleteCommand;
            }
        }

        public void Delete() {
            try {
                venueService.DeleteVenue(venue);
            } catch (Exception ex) {
                PlatformService.Instance.ShowErrorMessage(ex.Message, "Error");
            }
        }

    }

}
