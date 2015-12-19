using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander.ViewModel {
    public class DisplayDateTime : INotifyPropertyChanged {
        private DateTime dateTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public DisplayDateTime(DateTime dt) {
            DateTime = dt;
        }

        public DateTime DateTime {
            get {
                return dateTime;
            }
            set {
                if(dateTime != value) { 
                    dateTime = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateTime)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortDateString)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongDateString)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShortTimeString)));
                }
            }
        }

        public string ShortDateString {
            get {
                return DateTime.ToShortDateString();
            }
        }

        public string LongDateString {
            get {
                return DateTime.ToLongDateString();
            }
        }

        public string ShortTimeString {
            get {
                return DateTime.ToShortTimeString();
            }
        }

        public override string ToString() {
            return LongDateString;
        }
    }

    public class PerformanceManagementViewModel : INotifyPropertyChanged {
        private IPerformanceService performanceService;
        private IArtistService artistService;
        private IVenueService venueService;

        private DisplayDateTime currentDay;

        private DateTime timeInput;
        private Venue venueInput;
        private Artist artistInput;

        private ICommand createCommand;
        private ICommand exportHtmlCommand;
        private ICommand exportPdfCommand;
        private ICommand informArtistsCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public PerformanceManagementViewModel(IPerformanceService performanceService, IArtistService artistService, IVenueService venueService) {
            this.performanceService = performanceService;
            this.artistService = artistService;
            this.venueService = venueService;

            PerformancesForDay = new ObservableCollection<PerformanceViewModel>();
            Venues = new ObservableCollection<Venue>();
            Artists = new ObservableCollection<Artist>();
            CurrentDay = new DisplayDateTime(DateTime.Now);

            UpdatePerformancesForDay();
            UpdateVenues();
            UpdateArtists();

            DateTime now = DateTime.Now;
            timeInput = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            if(Venues.Count > 0) {
                VenueInput = Venues[0];
            }
            if(Artists.Count > 0) {
                ArtistInput = Artists[0];
            }

            CurrentDay.PropertyChanged += CurrentDay_PropertyChanged;
        }

        private void CurrentDay_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(DisplayDateTime.DateTime)) {
                UpdatePerformancesForDay();
            }
        }

        public DisplayDateTime CurrentDay {
            get {
                return currentDay;
            }
            set {
                if(currentDay != value) {
                    currentDay = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDay)));
                }
            }
        }

        public DateTime TimeInput {
            get {
                return timeInput;
            }
            set {
                if(timeInput != value) {
                    timeInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeInput)));
                }
            }
        }

        public Venue VenueInput {
            get {
                return venueInput;
            }
            set {
                if (venueInput != value) {
                    venueInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VenueInput)));
                }
            }
        }

        public Artist ArtistInput {
            get {
                return artistInput;
            }
            set {
                if (artistInput != value) {
                    artistInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArtistInput)));
                }
            }
        }

        public ObservableCollection<PerformanceViewModel> PerformancesForDay {
            get; set;
        }

        public ObservableCollection<Venue> Venues {
            get; set;
        }

        public ObservableCollection<Artist> Artists {
            get; set;
        }

        public ICommand CreateCommand {
            get {
                if(createCommand == null) {
                    createCommand = new RelayCommand((param) => {
                        try {
                            DateTime day = CurrentDay.DateTime;
                            DateTime date = new DateTime(day.Year, day.Month, day.Day, timeInput.Hour, 0, 0);
                            Performance p = performanceService.CreatePerformance(date, ArtistInput, VenueInput);
                            UpdatePerformancesForDay();
                            PlatformService.Instance.ShowInformationMessage("Performance added", "Success");
                        } catch(DataValidationException ex) {
                            PlatformService.Instance.ShowErrorMessage(ex.Message, "Error creating performance");
                        }
                    });
                }
                return createCommand;
            }
        }

        public ICommand ExportHtmlCommand {
            get {
                if(exportHtmlCommand == null) {
                    exportHtmlCommand = new RelayCommand((param) => {
                        Task.Run(() => {
                            DayProgramHtmlExporter exporter = new DayProgramHtmlExporter();
                            string path = MediaManager.Instance.GetFullPath(exporter.exportDayProgram(CurrentDay.DateTime), MediaType.Html);
                            string url = "file:///" + path.Replace('\\', '/');
                            System.Diagnostics.Process.Start(url);
                        });
                    });
                }
                return exportHtmlCommand;
            }
        }

        public ICommand ExportPdfCommand {
            get {
                if (exportPdfCommand == null) {
                    exportPdfCommand = new RelayCommand((param) => {
                        Task.Run(() => {
                            DayProgramPdfExporter exporter = new DayProgramPdfExporter();
                            string path = MediaManager.Instance.GetFullPath(exporter.exportDayProgram(CurrentDay.DateTime), MediaType.Pdf);
                            string url = "file:///" + path.Replace('\\', '/');
                            System.Diagnostics.Process.Start(url);
                        });
                    });
                }
                return exportPdfCommand;
            }
        }

        public ICommand InformArtistsCommand {
            get {
                if (informArtistsCommand == null) {
                    informArtistsCommand = new RelayCommand((param) => {
                        Task.Run(() => {
                            Config conf = Config.Get();
                            IMailer mailer = new SmtpMailer(conf.EmailSenderAddress, conf.SmtpServer, conf.SmtpPort, conf.SmtpUser, conf.SmtpPassword);
                            mailer.OnDayProgramChanged(CurrentDay.DateTime);
                        });
                    });
                }
                return informArtistsCommand;
            }
        }

        public void UpdatePerformancesForDay() {
            PerformancesForDay.Clear();
            Task.Run(() => {
                List<Performance> performances = performanceService.GetPerformancesForDay(CurrentDay.DateTime);
                foreach (var perf in performances) {
                    PerformanceViewModel vm = new PerformanceViewModel(this, performanceService, perf,
                        artistService.GetArtistById(perf.ArtistId), venueService.GetVenueById(perf.VenueId));
                    PlatformService.Instance.RunByUiThread(() => {
                        PerformancesForDay.Add(vm);
                    });
                }
            });
        }

        public void UpdateVenues() {
            Venues.Clear();
            foreach (var venue in venueService.GetAllVenues()) {
                Venues.Add(venue);
            }
        }

        public void UpdateArtists() {
            Artists.Clear();
            foreach (var artist in artistService.GetAllArtists()) {
                Artists.Add(artist);
            }
        }
    }
}
