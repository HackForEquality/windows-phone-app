using Caliburn.Micro;
using Microsoft.Devices;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
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
                ImagePath = settings["logo"] as Uri;
            }
            else
            {
                // Default
                ImagePath = new Uri("/Resources/Assets/Badges/White/YES_ImVoting.png", UriKind.Relative);
                settings["logo"] = ImagePath;
            }
        }

        #region Commands
        public async void ShutterButton()
        {
            // Set immediate viewfinder preview
            WriteableBitmap preview = new WriteableBitmap(mainView.ViewFinder, null);
            mainView.ViewFinderPreview.Source = preview;

            // Take picture
            var picStream = await mainView.ViewFinder.TakePicture();
            var pic = new WriteableBitmap(480, 640).FromStream(picStream);

            // Crop
            var croppedPic = pic.Crop(0, 80, 480, 480);

            // Add logo to image
            Rect cRect = new Rect(0, 0, pic.PixelWidth, pic.PixelHeight);
            WriteableBitmap logoBitmap = new WriteableBitmap(mainView.PreviewGrid, null);
            croppedPic.Blit(cRect, logoBitmap, cRect, WriteableBitmapExtensions.BlendMode.None);

            // Update preview
            mainView.Preview.Source = croppedPic;

            // Save
            croppedPic.SaveToMediaLibrary("Picture.jpg");

            // Cleanup
            pic = null;
            croppedPic = null;
        }
        
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
