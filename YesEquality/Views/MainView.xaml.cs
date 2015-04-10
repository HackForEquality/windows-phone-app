using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using Windows.Phone.Media.Capture;

namespace YesEquality.Views
{
    public partial class MainView : PhoneApplicationPage
    {
        public MainView()
        {
            InitializeComponent();

            ViewFinder.SensorLocation = CameraSensorLocation.Front;
            ViewFinder.Start();
        }
    }
}