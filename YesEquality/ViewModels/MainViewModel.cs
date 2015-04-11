using Caliburn.Micro;
using Microsoft.Devices;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using Windows.Phone.Media.Capture;
using YesEquality.Views;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : Screen
    {
        private readonly INavigationService navigationService;
        private MainView mainView;
        public Uri ImagePath { get; set; }

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        protected override void OnViewReady(object view)
        {
            mainView = view as MainView;
            
            // Start camera
            mainView.ViewFinder.SensorLocation = CameraSensorLocation.Front;
            mainView.ViewFinder.Start();

            // Use save logo selection
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("logo"))
            {
                ImagePath = new Uri(settings["logo"] as string, UriKind.Relative);
            }
            else
            {
                // Default
                ImagePath = new Uri("/Resources/Assets/Badges/White/YES_ImVoting.png", UriKind.Relative);
            }
        }

        #region Commands
        public void GoToAbout()
        {
            navigationService.UriFor<AboutViewModel>().Navigate();
        }

        public void GoToInfo()
        {
            navigationService.UriFor<InfoViewModel>().Navigate();
        }

        public void GoToBadges()
        {
            navigationService.UriFor<BadgeViewModel>().Navigate();
        }

        public bool CanSwitchCamera()
        {
            return PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) && PhotoCamera.IsCameraTypeSupported(CameraType.Primary);
        }

        public void SwitchCamera()
        {
            mainView.ViewFinder.SensorLocation = mainView.ViewFinder.SensorLocation == CameraSensorLocation.Front ? CameraSensorLocation.Back : CameraSensorLocation.Front;
        }
        #endregion

    }
}
