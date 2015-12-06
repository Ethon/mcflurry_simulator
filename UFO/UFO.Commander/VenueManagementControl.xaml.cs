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
            this.Loaded += (s, e) => {
                DataContext = new VenueListViewModel(ServiceFactory.CreateVenueService(new MYSQLDatabase("Server = localhost; Database = ufotest; Uid = root;")));
            };

        }

        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            // Disables the default mouse double-click action.
            e.Handled = true;
           
            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = ufoMap.ViewportPointToLocation(mousePosition);
            Pushpin current = (Pushpin)ufoMap.Children[0];
            current.Location = pinLocation;
        }
    }
}
