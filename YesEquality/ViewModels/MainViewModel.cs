using Caliburn.Micro;
using Microsoft.Devices;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using PropertyChanged;
using System;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Phone.Media.Capture;
using YesEquality.Views;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : Screen, IHandle<ShareMediaTask>
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        private MainView mainView;
        private string imagePath;
        
        public bool PrimaryAppBarVisible {get; set;}
        public bool SecondaryAppBarVisible { get; set; }
        public Uri ImagePath { get; set; }

        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            PrimaryAppBarVisible = false;
            SecondaryAppBarVisible = false;
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

        protected override async void OnViewLoaded(object view)
        {
            await Task.Delay(500);
            //SystemTray.IsVisible = true;
            PrimaryAppBarVisible = true;
        }

        #region Commands
        public bool CanTakePicture
        {
            get
            {
                return PrimaryAppBarVisible;
            }
        }
        
        public async void TakePicture()
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
            croppedPic.Blit(cRect, logoBitmap, cRect, WriteableBitmapExtensions.BlendMode.Alpha);

            // Update preview
            mainView.Preview.Source = croppedPic;

            PrimaryAppBarVisible = false;
            SecondaryAppBarVisible = !PrimaryAppBarVisible;

            // Save
            var picture = croppedPic.SaveToMediaLibrary("Picture.jpg");
            imagePath = picture.GetPath();

            // Cleanup
            pic = null;
            croppedPic = null;
        }

        public bool CanCacel
        {
            get
            {
                return true;
            }
        }

        public void Cancel()
        {
            mainView.ViewFinderPreview.Source = null;
            mainView.Preview.Source = null;

            PrimaryAppBarVisible = true;
            SecondaryAppBarVisible = !PrimaryAppBarVisible;
        }
        
        public void GoToSettings()
        {
            navigationService.UriFor<SettingsViewModel>().Navigate();
        }

        public void GoToInfo()
        {
            navigationService.UriFor<InfoViewModel>().Navigate();
        }

        public void GoToBadges()
        {
            navigationService.UriFor<BadgeViewModel>().Navigate();
        }

        public bool CanSwitchCamera
        {
            get
            {
                return PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) && PhotoCamera.IsCameraTypeSupported(CameraType.Primary);
            }
        }

        public void SwitchCamera()
        {
            mainView.ViewFinder.SensorLocation = mainView.ViewFinder.SensorLocation == CameraSensorLocation.Front ? CameraSensorLocation.Back : CameraSensorLocation.Front;
        }

        public bool CanShare
        {
            get
            {
                return true;
            }
        }

        public void Share()
        {
            eventAggregator.RequestTask<ShareMediaTask>(x => x.FilePath = imagePath);
        }

        public void Handle(ShareMediaTask message)
        {
        }
        #endregion

    }
}
