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
    /// Interaction logic for ArtistManagementControl.xaml
    /// </summary>
    public partial class ArtistManagementControl : UserControl {
        public ArtistManagementControl() {
            InitializeComponent();

            IDatabase db = new MYSQLDatabase("Server = localhost; Database = ufo; Uid = root;");
            SharedServices.Init(db);
            Alvm = new ArtistListViewModel(SharedServices.Instance.ArtistService, SharedServices.Instance.CategoryService);
            Clvm = new CategoryListViewModel(SharedServices.Instance.CategoryService);
            this.DataContext = this;
            dgArtists.DataContext = this;
        }

        public ArtistListViewModel Alvm {
            get; private set;
        }

        public CategoryListViewModel Clvm {
            get; private set;
        }
    }
}
