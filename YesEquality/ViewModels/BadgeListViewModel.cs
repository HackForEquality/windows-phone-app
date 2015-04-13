using Caliburn.Micro;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Diagnostics;
using YesEquality.Models;
using YesEquality.Views;
using System.Linq;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class BadgeListViewModel : Screen
    {
        public ObservableCollection<BadgeModel> Badges { get; set; }

        public BadgeListViewModel()
        {
        }
        protected override void OnViewLoaded(object view)
        {
            if (Badges != null)
            {
                var selected = Badges.Where(x => x.ImagePath == (Parent as BadgeViewModel).SelectedBadge.ImagePath);

                if (selected.Any())
                {
                    var badgeListView = GetView() as BadgeListView;
                    badgeListView.BadgeList.CheckedItems.Add(selected.First());
                }
            }
        }

        #region Commands
        public void ItemCheckedStateChanging(Telerik.Windows.Controls.ItemCheckedStateChangingEventArgs e)
        {
            // When new badge is checked, uncheck previous
            if (e.IsChecked)
            {
                var badgeListView = GetView() as BadgeListView;
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
