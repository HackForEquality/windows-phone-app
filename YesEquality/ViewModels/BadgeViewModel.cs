using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using YesEquality.Models;
using YesEquality.Views;

namespace YesEquality.ViewModels
{
    public class BadgeViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly INavigationService navigationService;
        private readonly BadgeListViewModel whiteBadges;
        private readonly BadgeListViewModel colourBadges;
        public BadgeModel SelectedBadge;

        public BadgeViewModel(INavigationService navigationService, BadgeListViewModel whiteBadges, BadgeListViewModel colourBadges)
        {
            this.navigationService = navigationService;

            this.whiteBadges = whiteBadges;
            this.colourBadges = colourBadges;

            this.whiteBadges.DisplayName = "simple";
            this.colourBadges.DisplayName = "colourful";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            // Setup pivot
            Items.Add(whiteBadges);
            Items.Add(colourBadges);
            ActivateItem(whiteBadges);

            // Set selected badge
            var imagePath = IsolatedStorageSettings.ApplicationSettings["logo"] as Uri;
            SelectedBadge = new BadgeModel() { ImagePath =  imagePath };

            // Set white badges
            var badges = createBadges("/Resources/Assets/Badges/White/");
            whiteBadges.Badges = new ObservableCollection<BadgeModel>(badges);

            // Set colour badges
            badges = createBadges("/Resources/Assets/Badges/Colour/");
            colourBadges.Badges = new ObservableCollection<BadgeModel>(badges);
        }

        private List<BadgeModel> createBadges(string path)
        {
            var badges = new List<BadgeModel>();
            badges.Add(new BadgeModel() { ImagePath = new Uri(path + "TA.png", UriKind.Relative) });
            badges.Add(new BadgeModel() { ImagePath = new Uri(path + "YES.png", UriKind.Relative) });
            badges.Add(new BadgeModel() { ImagePath = new Uri(path + "YES_ImVoting.png", UriKind.Relative) });
            badges.Add(new BadgeModel() { ImagePath = new Uri(path + "YES_Me.png", UriKind.Relative) });
            badges.Add(new BadgeModel() { ImagePath = new Uri(path + "YES_WereVoting.png", UriKind.Relative) });

            return badges;
        }
        #region Commands
        public void Save()
        {
            IsolatedStorageSettings.ApplicationSettings["logo"] = SelectedBadge.ImagePath;
            IsolatedStorageSettings.ApplicationSettings.Save();
            navigationService.GoBack();
        }
        
        public void cancel()
        {
            navigationService.GoBack();
        }
        #endregion
    }
}
