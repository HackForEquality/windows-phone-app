using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using YesEquality.Models;
using YesEquality.Views;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class BadgeListViewModel : Screen
    {
        public ObservableCollection<BadgeModel> Badges { get; set; }

        public BadgeListViewModel()
        {
        }

        #region Commands
        public void ItemCheckedStateChanging(Telerik.Windows.Controls.ItemCheckedStateChangingEventArgs e)
        {
            var badgeListView = GetView() as BadgeListView;

            // When new badge is checked, uncheck previous
            if (e.IsChecked)
            {
                if (badgeListView.BadgeList.CheckedItems != null)
                {
                    badgeListView.BadgeList.CheckedItems.Clear();
                }
            }

            // Set new checked item
            var item = e.Item as BadgeModel;
            (Parent as BadgeViewModel).SelectedBadge = new BadgeModel() { ImagePath = item.ImagePath };
        }
        #endregion
    }
}
