using Swk5.MediaAnnotator.ViewModel;
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
    public class VenueListViewModel :INotifyPropertyChanged {
        private IVenueService venueService;
        private VenueViewModel currentVenue;
        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand addCommand;
        private ICommand deleteCommand;

        public ObservableCollection<VenueViewModel> Venues { get; set; }

        public VenueListViewModel(IVenueService venueService) {
            this.venueService = venueService;
            this.Venues = new ObservableCollection<VenueViewModel>();
            LoadVenues();
            
        }

        //public void LoadVenues() {
        //    Venues.Clear();
        //    Console.WriteLine("STARTE MIT LOADING");
        //    Console.WriteLine(venueService.GetAllVenues().Count);
        //    foreach (Venue item in venueService.GetAllVenues()) {

        //        Venues.Add(new VenueViewModel(venueService, item));
        //    }
        //}

        public async void LoadVenues() {
            CurrentVenue = null;
            Venues.Clear();
            IEnumerator<Venue> e = venueService.GetAllVenues().GetEnumerator();

            while (await Task.Factory.StartNew(
                    () => e.MoveNext())) {
                Venues.Add(new VenueViewModel(venueService, e.Current));
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


        public ICommand AddCommand {
            get {
                if (addCommand == null) {
                    addCommand = new RelayCommand(param => Venues.Add(new VenueViewModel(venueService,venueService.CreateVenue("Demo","D0",1,1))));
                }
                return addCommand;
            }
        }
        public ICommand DeleteCommand {
            get {
                if (deleteCommand == null) {
                    deleteCommand = new RelayCommand(param => {
                        try {
                            venueService.DeleteVenue(CurrentVenue.venue);
                            Venues.Remove(CurrentVenue);
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Error",  MessageBoxButton.OK,MessageBoxImage.Warning);
                        }
                });
                    
                    
                }
                return deleteCommand;
            }
        }




    }
    public class VenueViewModel : INotifyPropertyChanged {

        private IVenueService venueService;
        public Venue venue;


        
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


    }
}
