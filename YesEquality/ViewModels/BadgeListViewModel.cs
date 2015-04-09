using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using YesEquality.Models;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class BadgeListViewModel : Screen
    {
        public ObservableCollection<BadgeModel> Badges { get; set; }
        
        public BadgeListViewModel()
        {
            //Badges = new ObservableCollection<BadgeModel>();
        }

        #region Commands

        #endregion
    }
}
