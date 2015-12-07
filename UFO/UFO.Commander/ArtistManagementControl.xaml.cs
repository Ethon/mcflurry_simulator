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

        private const string PICTURE_FILTER = "Image Files |*.jpeg; *.png; *.jpg; *.gif;";
        private const string VIDEO_FILTER = "Video files |*.wmv; *.mp4; *.avi; *.flv;";

        public ArtistManagementControl() {
            InitializeComponent();

            Amvm = new ArtistManagementViewModel(SharedServices.Instance.ArtistService,
                SharedServices.Instance.CategoryService, SharedServices.Instance.CountryService);
            this.DataContext = this;
        }

        public ArtistManagementViewModel Amvm {
            get; private set;
        }

        private void PicturePathColumn_MouseDown(object sender, MouseButtonEventArgs e) {
            TextBlock block = sender as TextBlock;
            ArtistViewModel model = block.DataContext as ArtistViewModel;
            string newFile = PlatformService.Instance.PickFile("Pick picture file", PICTURE_FILTER);
            if(newFile != null) {
                model.PicturePath = MediaManager.Instance.RootPicture(newFile);
            }
        }

        private void VideoPathColumn_MouseDown(object sender, MouseButtonEventArgs e) {
            TextBlock block = sender as TextBlock;
            ArtistViewModel model = block.DataContext as ArtistViewModel;
            string newFile = PlatformService.Instance.PickFile("Pick video file", VIDEO_FILTER);
            if (newFile != null) {
                model.VideoPath = MediaManager.Instance.RootVideo(newFile);
            }
        }

        private void PicturePathInput_MouseDown(object sender, MouseButtonEventArgs e) {
            string newFile = PlatformService.Instance.PickFile("Pick picture file", PICTURE_FILTER);
            if(newFile != null) {
                Amvm.PicturePathInput = MediaManager.Instance.RootPicture(newFile);
            }
        }

        private void VideoPathInput_MouseDown(object sender, MouseButtonEventArgs e) {
            string newFile = PlatformService.Instance.PickFile("Pick video file", VIDEO_FILTER);
            if (newFile != null) {
                Amvm.VideoPathInput = MediaManager.Instance.RootVideo(newFile);
            }
        }
    }
}
