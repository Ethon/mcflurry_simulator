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
    public class DisplayDateTime {
        public DisplayDateTime(DateTime dt) {
            DateTime = dt;
        }

        public DateTime DateTime { get; private set; }

        public string ShortDateString {
            get {
                return DateTime.ToShortDateString();
            }
        }

        public override string ToString() {
            return ShortDateString;
        }
    }

    public class PerformanceManagementViewModel : INotifyPropertyChanged {
        private IVenueService venueService;

        private DisplayDateTime currentDay;
        private ICommand showPreviousDayCommand;
        private ICommand showNextDayCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public PerformanceManagementViewModel(IVenueService venueService) {
            this.venueService = venueService;
            PerformancesForDay = new ObservableCollection<PerformanceViewModel>();
            CurrentDay = new DisplayDateTime(DateTime.Now);

            UpdateCurrentDay();
        }

        public DisplayDateTime CurrentDay {
            get {
                return currentDay;
            }
            private set {
                if(currentDay != value) {
                    currentDay = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDay)));
                }
            }
        }

        public ObservableCollection<PerformanceViewModel> PerformancesForDay {
            get; set;
        }

        public ICommand ShowPreviousDayCommand {
            get {
                if (showPreviousDayCommand == null) {
                    showPreviousDayCommand = new RelayCommand((param) => {
                        PlatformService.Instance.ShowInformationMessage("prev", "test");
                    });
                }
                return showPreviousDayCommand;
            }
        }

        public ICommand ShowNextDayCommand {
            get {
                if(showNextDayCommand == null) {
                    showNextDayCommand = new RelayCommand((param) => {
                        PlatformService.Instance.ShowInformationMessage("next", "test");
                    });
                }
                return showNextDayCommand;
            }
        }

        public void UpdateCurrentDay() {
        }
    }
}
