using Caliburn.Micro;
using Microsoft.Devices;
using System;

namespace YesEquality.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly INavigationService navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
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
            
        }
        #endregion

    }
}
