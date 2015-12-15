using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UFO.Commander.ViewModel;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander {
    /// <summary>
    /// Interaction logic for PerformanceManagementControl.xaml
    /// </summary>
    public partial class PerformanceManagementControl : UserControl, IVenueListener, IArtistListener {
        public PerformanceManagementControl() {
            InitializeComponent();
            SharedServices.Instance.ArtistService.AddListener(this);
            SharedServices.Instance.VenueService.AddListener(this);

            SharedServices services = SharedServices.Instance;
            PmVm = new PerformanceManagementViewModel(services.PerformanceService,
                services.ArtistService, services.VenueService);
            DataContext = this;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        public PerformanceManagementViewModel PmVm { get; private set; }

        public void OnArtistCreation(Artist artist) {
            PmVm.UpdateArtists();
        }

        public void OnArtistDeletion(Artist artist) {
            PmVm.UpdateArtists();
        }

        public void OnArtistUpdate(Artist artist) {
            PmVm.UpdateArtists();
        }

        public void OnVenueCreation(Venue venue) {
            PmVm.UpdateVenues();
        }

        public void OnVenueDeletion(Venue venue) {
            PmVm.UpdateVenues();
        }

        public void OnVenueUpdate(Venue venue) {
            PmVm.UpdateVenues();
        }
    }
}
