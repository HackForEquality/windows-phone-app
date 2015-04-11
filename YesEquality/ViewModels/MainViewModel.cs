using Caliburn.Micro;
using Microsoft.Devices;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using YesEquality.Models.Messages;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;
        private readonly INavigationService navigationService;

        public Uri ImagePath { get; set; }

        public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
        }

        protected override void OnViewReady(object view)
        {
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

        public void SwitchCamera()
        {
            eventAggregator.PublishOnUIThread(new CommandMessage(Commands.SwitchCamera));
        }
        #endregion

    }
}
