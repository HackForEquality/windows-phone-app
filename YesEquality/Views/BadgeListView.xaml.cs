using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Caliburn.Micro;
using YesEquality.Models.Messages;
using Telerik.Windows.Controls;

namespace YesEquality.Views
{
    public partial class BadgeListView : UserControl, IHandle<CommandMessage>
    {
        private readonly IEventAggregator eventAggregator;

        public BadgeListView()
        {
            InitializeComponent();
            
            // Setup messaging
            AppBootstrapper bootstrapper = Application.Current.Resources["bootstrapper"] as AppBootstrapper;
            IEventAggregator eventAggregator = bootstrapper.container.GetAllInstances(typeof(IEventAggregator)).FirstOrDefault() as IEventAggregator;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            // Setup tilt animation
            InteractionEffectManager.AllowedTypes.Add(typeof(RadDataBoundListBoxItem));
        }

        public void Handle(CommandMessage message)
        {
            // Clear checked items
            if (message.Command == Commands.ClearCheckedItems)
            {
                BadgeList.CheckedItems.Clear();
            }
        }
    }
}
