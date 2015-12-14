using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UFO.Server;

namespace UFO.Commander {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IPlatformService {
        protected override void OnStartup(StartupEventArgs e) {
            PlatformService.Instance = this;
            Config config = Config.Load();

            string connectionString = String.Format("Server = {0}; Database = {1}; Uid = {2};",
                config.DatabaseServer, config.DatabaseName, config.DatabaseUser);
            IDatabase db = null;
            try { 
                db = new MYSQLDatabase(connectionString);
                
            } catch(Exception ex) {
                PlatformService.Instance.ShowErrorMessage(
                    "Could not connect to database '" + connectionString + "'\n\n" + ex.Message,
                    "Database connection failure");
                Environment.Exit(-1);
            }

            SharedServices.Init(db);
            base.OnStartup(e);
        }

        public string PickFile(string title, string filter, string rootFolder) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Title = title;
            if (rootFolder != null && rootFolder.Length > 0) {
                openFileDialog.InitialDirectory = rootFolder;
            }

            if (openFileDialog.ShowDialog() == true) {
                return openFileDialog.FileName;
            }
            return null;
        }

        public string PickFolder(string defaultPath) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if(defaultPath != null && defaultPath.Length > 0) {
                dialog.SelectedPath = defaultPath;
            }
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                return dialog.SelectedPath;
            }
            return null;
        }

        public void ShowInformationMessage(string message, string caption) {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(message, caption, button, icon);
        }

        public void ShowErrorMessage(string message, string caption) {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, caption, button, icon);
        }

        public bool WarnAndAskForConfirmation(string message, string caption) {
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            return MessageBox.Show(message, caption, button, icon) == MessageBoxResult.Yes;
        }

        public void RunByUiThread(Action action) {
            Dispatcher.Invoke(action);
        }
    }
}
