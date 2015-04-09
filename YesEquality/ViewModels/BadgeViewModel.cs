using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YesEquality.Models;

namespace YesEquality.ViewModels
{
    public class BadgeViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly BadgeListViewModel whiteBadges;
        private readonly BadgeListViewModel colourBadges;

        public BadgeViewModel(BadgeListViewModel whiteBadges, BadgeListViewModel colourBadges)
        {
            this.whiteBadges = whiteBadges;
            this.colourBadges = colourBadges;

            this.whiteBadges.DisplayName = "simple";
            this.colourBadges.DisplayName = "colourful";
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Items.Add(whiteBadges);
            Items.Add(colourBadges);
            
            ActivateItem(whiteBadges);

            var badges = createBadges("/Resources/Assets/Badges/White/");
            whiteBadges.Badges = new ObservableCollection<BadgeModel>(badges);

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

        #endregion
    }
}
