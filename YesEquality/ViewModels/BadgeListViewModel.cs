using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using YesEquality.Models;
using YesEquality.Models.Messages;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class BadgeListViewModel : Screen
    {
        private readonly IEventAggregator eventAggregator;
        public ObservableCollection<BadgeModel> Badges { get; set; }

        public BadgeListViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        #region Commands
        public void ItemCheckedStateChanging(Telerik.Windows.Controls.ItemCheckedStateChangingEventArgs e)
        {
            // When new badge is checked, uncheck previous
            if (e.IsChecked)
            {
                eventAggregator.PublishOnUIThread(new CommandMessage(Commands.ClearCheckedItems));
            }

            // Set new checked item
        }
        #endregion
    }
}
