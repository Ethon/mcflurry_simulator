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
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : Window {
        public LoginControl() {
            InitializeComponent();
            LVm = new LoginViewModel(SharedServices.Instance.UserService);
            this.DataContext = this;
        }


        public LoginViewModel LVm
        {
            get; private set;
        }

        public LoginViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }
    }
}
