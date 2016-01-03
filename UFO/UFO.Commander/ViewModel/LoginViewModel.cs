using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UFO.Server;

namespace UFO.Commander.ViewModel {
    public class LoginViewModel : INotifyPropertyChanged {
        //https://social.msdn.microsoft.com/forums/vstudio/en-US/72d4eced-f3b8-4898-a7ff-5f8f6e763f0e/wpf-and-mvvm-with-login-authentication
        private bool isAuthenticated;
        private string loginInfo;
        private string username;
        private string password;
        private IUserService userService;
        private ICommand loginCommand;
        private ICommand shutDownCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        

        public LoginViewModel(IUserService userService) {
            isAuthenticated = false;
            this.userService = userService;
            this.username = "";
            this.password = "";
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set
            {
                if (this.isAuthenticated != value) {
                    isAuthenticated = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAuthenticated)));
                }
            }
        }

        public string LoginInfo
        {
            get { return loginInfo; }
            set
            {
                if (this.loginInfo != value) {
                    loginInfo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoginInfo)));
                }
            }
        }

        public string UserName
        {
            get { return username; }
            set
            {
                if (this.username != value) {
                    username = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                }

            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (this.password != value) {
                    password = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null) {
                    loginCommand = new RelayCommand((param) => {
                        Login((LoginControl)param);
                    });
                }
                return loginCommand;
            }
        }

        public ICommand ShutDownCommand
        {
            get
            {
                if (shutDownCommand == null) {
                    shutDownCommand = new RelayCommand((param) => {
                        Application.Current.Shutdown();
                    });
                }
                return shutDownCommand;
            }
        }

        public void Login(LoginControl loginControl) {
            try {
                IsAuthenticated = userService.CheckCredentials(username, Password);
                if (IsAuthenticated) {
                    loginControl.Close();
                } else {
                    LoginInfo = "Error: Wrong username or password";
                }
            } catch (Exception ex) {
                LoginInfo = "Error: " +ex.Message;
            }
        }

    }
}
