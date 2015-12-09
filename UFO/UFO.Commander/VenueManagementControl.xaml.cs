using Microsoft.Maps.MapControl.WPF;
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
using UFO.Commander.ViewModel;
using UFO.Server;

namespace UFO.Commander {
    /// <summary>
    /// Interaction logic for VenueManagementControl.xaml
    /// </summary>
    public partial class VenueManagementControl : UserControl {



        public VenueManagementControl() {
            InitializeComponent();
            VmVm = new VenueManagementViewModel(SharedServices.Instance.VenueService);
            this.DataContext = this;
        }

        public VenueManagementViewModel VmVm
        {
            get; private set;
        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            //Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = venueMap.ViewportPointToLocation(mousePosition);
            Pushpin current = (Pushpin)venueMap.Children[0];
            Console.WriteLine(pinLocation.Latitude);
            current.Location.Latitude = pinLocation.Latitude;
            current.Location.Longitude = pinLocation.Longitude;
        }

        Vector _mouseToMarker;
        private bool _dragPin;
        Location tempLocation;
        public Pushpin SelectedPushpin { get; set; }

        void pin_MouseDown(object sender, MouseButtonEventArgs e) {
            
            e.Handled = true;
            SelectedPushpin = sender as Pushpin;
            _dragPin = true;
            
            _mouseToMarker = Point.Subtract(
              venueMap.LocationToViewportPoint(SelectedPushpin.Location),
              e.GetPosition(venueMap));
            tempLocation = SelectedPushpin.Location;
        }

        private void map_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (_dragPin && SelectedPushpin != null) {
                    SelectedPushpin.Location = venueMap.ViewportPointToLocation(
                      Point.Add(e.GetPosition(venueMap), _mouseToMarker));
                    e.Handled = true;
                }
            }
        }
        void pin_MouseUp(object sender, MouseButtonEventArgs e) {
            e.Handled = true;
            _dragPin = false;
            
            if (SelectedPushpin.Location != tempLocation) {
                if (PlatformService.Instance.WarnAndAskForConfirmation(
                    "Do you want to change the coordinates of the venue '" + VmVm.CurrentVenue.Name + "'?", "Confirm coordination change")) {
                    VmVm.CurrentVenue.Location = SelectedPushpin.Location;
                } else {
                    VmVm.CurrentVenue.Location = tempLocation;
                }
            }

        }

    }
}
