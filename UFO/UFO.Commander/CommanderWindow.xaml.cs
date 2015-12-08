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
using System.Windows.Shapes;
using UFO.Commander.ViewModel;

namespace UFO.Commander {
    /// <summary>
    /// Interaction logic for CommanderWindow.xaml
    /// </summary>
    public partial class CommanderWindow : Window {
        public CommanderWindow() {
            InitializeComponent();

            Cvm = new CommanderViewModel();
            this.DataContext = this;
        }

        private void MediaRootTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            TextBox box = sender as TextBox;
            string newFolder = PlatformService.Instance.PickFolder(null);
            if(newFolder != null) {
                Cvm.MediaRootPath = newFolder;
            }
        }

        public CommanderViewModel Cvm { get; private set; }
    }
}
