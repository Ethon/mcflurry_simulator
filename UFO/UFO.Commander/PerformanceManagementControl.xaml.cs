using System;
using System.Collections.Generic;
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
        }

        public void OnArtistCreation(Artist artist) {
        }

        public void OnArtistDeletion(Artist artist) {
        }

        public void OnArtistUpdate(Artist artist) {
        }

        public void OnVenueCreation(Venue venue) {
        }

        public void OnVenueDeletion(Venue venue) {
        }

        public void OnVenueUpdate(Venue venue) {
        }
    }
}
