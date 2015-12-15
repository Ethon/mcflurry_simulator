using Microsoft.Maps.MapControl.WPF;
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
    public class VenueViewModel : INotifyPropertyChanged {

        private IVenueService venueService;
        public Venue venue;
        private ICommand deleteCommand;


        public event PropertyChangedEventHandler PropertyChanged;

        public VenueViewModel(IVenueService venueService, Venue venue) {
            this.venueService = venueService;
            this.venue = venue;
        }

        public uint Id
        {
            get { return venue.Id; }
        }

        public string ShortCut
        {
            get { return venue.Shortcut; }
            set
            {
                if (venue.Shortcut != value) {
                    venue.Shortcut = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortCut)));
                }
            }
        }
        public string Name
        {
            get { return venue.Name; }
            set
            {
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

        public double Longitude
        {
            get { return venue.Longitude; }
            set
            {
                if (venue.Longitude != value) {
                    venue.Longitude = value;
                    venueService.UpdateVenue(venue);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
                }
            }
        }
        public double Latitude
        {
            get { return venue.Latitude; }
            set
            {
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
