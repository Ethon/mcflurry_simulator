using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class PerformanceViewModel : INotifyPropertyChanged {
        private IPerformanceService performanceService;

        private Performance performance;
        private Artist artist;
        private Venue venue;

        public event PropertyChangedEventHandler PropertyChanged;

        public PerformanceViewModel(IPerformanceService performanceService,
            Performance performance, Artist artist, Venue venue) {
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
                if(performance.Date != value) {
                    performance.Date = value;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date)));
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
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Venue)));
                }
            }
        }

        public Artist Artist {
            get {
                return Artist;
            }
            set {
                if(artist != value) {
                    artist = value;
                    performance.ArtistId = artist.Id;
                    Update();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Artist)));
                }
            }
        }

        public void Update() {
            performanceService.UpdatePerformance(performance);
        }
    }
}
