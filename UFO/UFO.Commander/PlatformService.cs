﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UFO.Commander {
    public interface IPlatformService {
        string PickFile(string title, string filter);
        string PickFolder();
        void ShowInformationMessage(string message, string caption);
        void ShowErrorMessage(string message, string caption);
        bool WarnAndAskForConfirmation(string message, string caption);
    }

    public class PlatformService {
        public static IPlatformService Instance { get; set; }
    }
}
