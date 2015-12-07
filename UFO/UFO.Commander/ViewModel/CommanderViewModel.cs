using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Commander.ViewModel {
    public class CommanderViewModel : INotifyPropertyChanged {
        private string mediaRootPath;

        public event PropertyChangedEventHandler PropertyChanged;

        public string MediaRootPath {
            get {
                if(mediaRootPath == null) {
                    mediaRootPath = Config.Get().MediaRootPath;
                }
                return mediaRootPath;
            }
            set {
                mediaRootPath = value;
                MediaManager.Instance.RootPath = value;
                Config.Get().MediaRootPath = value;
                Config.Save();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MediaRootPath)));
            }
        }

        
    }
}
