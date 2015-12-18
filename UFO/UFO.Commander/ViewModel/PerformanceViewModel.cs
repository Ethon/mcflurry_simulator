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
    public class PerformanceViewModel : INotifyPropertyChanged {
        private PerformanceManagementViewModel pmvm;

        private IPerformanceService performanceService;

        private Performance performance;
        private Artist artist;
        private Venue venue;

        private ICommand deleteCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public PerformanceViewModel(PerformanceManagementViewModel pmvm, IPerformanceService performanceService,
            Performance performance, Artist artist, Venue venue) {

            this.pmvm = pmvm;

            this.performanceService = performanceService;

            this.performance = performance;
            this.artist = artist;
            this.venue = venue;
        }

        public uint Id {
            get {
                return performance.Id;
            }
        }

        public DateTime Date {
            get {
                return performance.Date;
            }
            set {
                DateTime old = performance.Date;
                if(performance.Date != value) {
                    performance.Date = value;
                    if(Update()) {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
                    }
                }
                if(old.Year != value.Year || old.Month != value.Month || old.Day != value.Day) {
                    pmvm.UpdatePerformancesForDay();
                    pmvm.CurrentDay.DateTime = value;
                }
            }
        }

        public Venue Venue {
            get {
                return venue;
            }
            set {
                if(venue != value) {
                    venue = value;
                    performance.VenueId = venue.Id;
                    if (Update()) {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Venue)));
                    }
                }
            }
        }

        public Artist Artist {
            get {
                return artist;
            }
            set {
                if(artist != value) {
                    artist = value;
                    performance.ArtistId = artist.Id;
                    if (Update()) {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Artist)));
                    }
                }
            }
        }

        public ICommand DeleteCommand {
            get {
                if (deleteCommand == null) {
                    deleteCommand = new RelayCommand((param) => {
                        if (PlatformService.Instance.WarnAndAskForConfirmation(
                                "Do you really want to delete the performance?", "Confirm deletion")) {
                            try {
                                Delete();
                            } catch (DataValidationException ex) {
                                PlatformService.Instance.ShowErrorMessage(ex.Message, "Error deleting performance");
                                return;
                            }
                            PerformanceManagementViewModel pmvm = param as PerformanceManagementViewModel;
                            pmvm.UpdatePerformancesForDay();
                        }
                    });
                }
                return deleteCommand;
            }
        }

        public bool Update() {
            try {
                performanceService.UpdatePerformance(performance);
                return true;
            } catch(DataValidationException ex) {
                PlatformService.Instance.ShowErrorMessage(ex.Message, "Error updating performance");
            }
            return false;
        }

        public void Delete() {
            performanceService.DeletePerformance(performance);
        }
    }
}
