using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Commander {
    public class MediaManager {
        private const string ICON_DIR = "/icon/";
        private const string PICTURE_DIR = "/picture/";
        private const string VIDEO_DIR = "/video/";

        private string rootPath;

        private static MediaManager instance;

        public static MediaManager Instance {
            get {
                if(instance == null) {
                    instance = new MediaManager(Config.Get().MediaRootPath);
                }
                return instance;
            }
        }

        private void AssertIsFile(string path) {
            FileAttributes attr = File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory)) {
                throw new ArgumentException("Only files  for this operation");
            }
        }

        private void CreateDirectoryStructure(string root) {
            Directory.CreateDirectory(root + ICON_DIR);
            Directory.CreateDirectory(root + PICTURE_DIR);
            Directory.CreateDirectory(root + VIDEO_DIR);
        }

        public MediaManager(string root) {
            RootPath = root;
        }

        public string RootPath {
            get {
                return rootPath;
            }
            set {
                FileAttributes attr = File.GetAttributes(value);
                if(!attr.HasFlag(FileAttributes.Directory)) {
                    throw new ArgumentException("Root path must be a directory");
                }
                CreateDirectoryStructure(value);
                rootPath = value;
            }
        }

        public bool IsFileInIconDir(string path) {
            return path.StartsWith(RootPath + ICON_DIR);
        }

        public bool IsFileInPictureDir(string path) {
            return path.StartsWith(RootPath + PICTURE_DIR);
        }

        public bool IsFileInVideoDir(string path) {
            return path.StartsWith(RootPath + VIDEO_DIR);
        }

        public string RootIcon(string path) {
            AssertIsFile(path);
            string fileName = Path.GetFileName(path);
            if (IsFileInIconDir(path)) {
                return fileName;
            }

            string newPath = RootPath + ICON_DIR + fileName;
            if (!File.Exists(newPath) || PlatformService.Instance.WarnAndAskForConfirmation(
                    "Do you really want to overwrite icon '" + fileName + "'", "Overwrite file")) {
                File.Copy(path, Path.GetDirectoryName(newPath), true);
            }
            return fileName;
        }

        public string RootPicture(string path) {
            AssertIsFile(path);
            string fileName = Path.GetFileName(path);
            if (IsFileInPictureDir(path)) {
                return fileName;
            }

            string newPath = RootPath + PICTURE_DIR + fileName;
            if (!File.Exists(newPath) || PlatformService.Instance.WarnAndAskForConfirmation(
                    "Do you really want to overwrite picture '" + fileName + "'", "Overwrite file")) {
                File.Copy(path, newPath, true);
            }
            return fileName;
        }

        public string RootVideo(string path) {
            AssertIsFile(path);
            string fileName = Path.GetFileName(path);
            if (IsFileInVideoDir(path)) {
                return fileName;
            }

            string newPath = RootPath + VIDEO_DIR + fileName;
            if (!File.Exists(newPath) || PlatformService.Instance.WarnAndAskForConfirmation(
                    "Do you really want to overwrite video '" + fileName + "'", "Overwrite file")) {
                File.Copy(path, newPath, true);
            }
            return fileName;
        }
    }
}
