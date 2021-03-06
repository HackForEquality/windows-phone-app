﻿using Caliburn.Micro;
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
using YesEquality.Helpers;
using System.ComponentModel;

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

            // Create badge list
            var badges = new List<Uri>();
            badges.AddRange(createBadges("/Resources/Assets/Badges/White/"));
            badges.AddRange(createBadges("/Resources/Assets/Badges/Colour/"));
            mainView.BadgePicker.Badges = badges;

            // Handle hardware camera buttons
            CameraButtons.ShutterKeyHalfPressed += async (s, e) =>
            {
                await mainView.ViewFinder.FocusAsync();
            };

            CameraButtons.ShutterKeyPressed += (s, e) =>
            {
                if (PrimaryAppBarVisible) TakePicture();
            };

            // Preload view, hack to fix missing page transition when page is first viewed
            var cacheView = new InfoView();
        }

        protected override async void OnViewLoaded(object view)
        {
            // Add reminders if not setup
            if (!ReminderHelper.IsSetup)
            {
                ReminderHelper.Setup();
            }

            await Task.Delay(500);
            PrimaryAppBarVisible = true;

            // Show badge tooltip once
            if (SettingsHelper.ShowBadgeTooltip)
            {
                mainView.BadgePicker.ShowTooltip();
                SettingsHelper.ShowBadgeTooltip = false;
            }
        }

        public void OnGoBack(CancelEventArgs e)
        {
            if (SecondaryAppBarVisible)
            {
                e.Cancel = true;
                Cancel();
            }
        }  

        private List<Uri> createBadges(string path)
        {
            var badges = new List<Uri>();
#if DEBUG
            var votingDay = new DateTime(2015, 4, 22, 0, 0, 0);
#else
            var votingDay = new DateTime(2015, 5, 22, 0, 0, 0);
#endif
            var result = DateTime.Compare(DateTime.UtcNow, votingDay);
            
            // After voting day
            if (result >= 0)
            {
                badges.Add(new Uri(path + "ItsAYes.png", UriKind.Relative));
                badges.Add(new Uri(path + "IVoted.png", UriKind.Relative));
                badges.Add(new Uri(path + "YesThankYou.png", UriKind.Relative));
            }
            
            // Before voting day
            badges.Add(new Uri(path + "YES_ImVoting.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES_WeAreVoting.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES_Me.png", UriKind.Relative));
            badges.Add(new Uri(path + "YES.png", UriKind.Relative));
            badges.Add(new Uri(path + "TA.png", UriKind.Relative));

            return badges;
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

            mainView.CameraFlash.Begin();
            mainView.HideShutterButton.Begin();
            await Task.Delay(300);

            // Add logo to image
            Rect cRect = new Rect(0, 0, preview.PixelWidth, preview.PixelHeight);
            WriteableBitmap logoBitmap = new WriteableBitmap(mainView.BadgePicker, null);
            preview.Blit(cRect, logoBitmap, cRect, WriteableBitmapExtensions.BlendMode.Alpha);

            // Update preview
            mainView.Preview.Source = preview;

            // Swtich appbar
            PrimaryAppBarVisible = false;
            SecondaryAppBarVisible = !PrimaryAppBarVisible;

            // Save
            var picture = preview.SaveToMediaLibrary("Picture.jpg");
            imagePath = picture.GetPath();

            // Cleanup
            preview = null;
            logoBitmap = null;
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

            mainView.ShowShutterButton.Begin();

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
