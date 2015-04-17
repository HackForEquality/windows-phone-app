using Caliburn.Micro;
using Microsoft.Devices;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Phone.Media.Capture;
using YesEquality.Models;
using YesEquality.Views;
using YesEquality.Extensions;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : Screen, IHandle<ShareMediaTask>
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        private MainView mainView;
        private string imagePath;
        private List<Uri> badgeList;

        public bool PrimaryAppBarVisible {get; set;}
        public bool SecondaryAppBarVisible { get; set; }
        public Uri SelectedBadge { get; set; }

        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            PrimaryAppBarVisible = false;
            SecondaryAppBarVisible = false;
        }

        protected override void OnViewReady(object view)
        {
            // Start camera
            mainView = view as MainView;
            mainView.ViewFinder.SensorLocation = CameraSensorLocation.Front;
            mainView.ViewFinder.Start();

            // Use save logo selection
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("badge"))
            {
                SelectedBadge = settings["badge"] as Uri;
            }
            else
            {
                // Default
                SelectedBadge = new Uri("/Resources/Assets/Badges/White/YES_ImVoting.png", UriKind.Relative);
                settings["badge"] = SelectedBadge;
            }

            // Create badge list
            badgeList = new List<Uri>();
            badgeList.AddRange(createBadges("/Resources/Assets/Badges/White/"));
            badgeList.AddRange(createBadges("/Resources/Assets/Badges/Colour/"));

            // Preload view, hack to fix missing page transition when page is first viewed
            var cacheView = new InfoView();
        }

        protected override async void OnViewLoaded(object view)
        {
            await Task.Delay(500);
            //SystemTray.IsVisible = true;
            PrimaryAppBarVisible = true;
        }

        private List<Uri> createBadges(string path)
        {
            var badges = new List<Uri>();
            badges.Add(new Uri(path + "TA.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES_ImVoting.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES_Me.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES_WereVoting.png", UriKind.Relative));

            return badges;
        }

        private Uri nextBadge()
        {
            var index = badgeList.IndexOf(SelectedBadge);
            if (badgeList.Count == (index+1))
            {
                return badgeList[0];
            }
            else
            {
                return badgeList[index+1];
            }
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

        public async void NextBadge()
        {
            SelectedBadge = nextBadge();
            var view = GetView() as MainView;
            await view.ImageBounce.BeginAsync().ConfigureAwait(false);
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
