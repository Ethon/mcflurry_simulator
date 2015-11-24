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
    }
}
