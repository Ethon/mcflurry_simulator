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

        private void LatitudeColumn_MouseDown(object sender, MouseButtonEventArgs e) {
            //TextBlock block = sender as TextBlock;
            //ArtistViewModel model = block.DataContext as ArtistViewModel;
            //string newFile = PlatformService.Instance.PickFile("Pick picture file", PICTURE_FILTER);
            //if (newFile != null) {
            //    model.PicturePath = MediaManager.Instance.RootPicture(newFile);
            //}
        }

        private void LongitudeColumn_MouseDown(object sender, MouseButtonEventArgs e) {
            //TextBlock block = sender as TextBlock;
            //ArtistViewModel model = block.DataContext as ArtistViewModel;
            //string newFile = PlatformService.Instance.PickFile("Pick video file", VIDEO_FILTER);
            //if (newFile != null) {
            //    model.VideoPath = MediaManager.Instance.RootVideo(newFile);
            //}
        }

        private void LatitudeInput_MouseDown(object sender, MouseButtonEventArgs e) {
            //string newFile = PlatformService.Instance.PickFile("Pick picture file", PICTURE_FILTER);
            //if (newFile != null) {
            //    Amvm.PicturePathInput = MediaManager.Instance.RootPicture(newFile);
            //}
        }

        private void LongitudeInput_MouseDown(object sender, MouseButtonEventArgs e) {
            //string newFile = PlatformService.Instance.PickFile("Pick video file", VIDEO_FILTER);
            //if (newFile != null) {
            //    Amvm.VideoPathInput = MediaManager.Instance.RootVideo(newFile);
            //}
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
            current.Location = pinLocation;
        }
    }
}
