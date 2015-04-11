using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using Windows.Phone.Media.Capture;
using Caliburn.Micro;
using YesEquality.Models.Messages;

namespace YesEquality.Views
{
    public partial class MainView : PhoneApplicationPage, IHandle<CommandMessage>
    {
        private readonly IEventAggregator eventAggregator;

        public MainView()
        {
            InitializeComponent();

            // Setup messaging
            AppBootstrapper bootstrapper = Application.Current.Resources["bootstrapper"] as AppBootstrapper;
            IEventAggregator eventAggregator = bootstrapper.container.GetAllInstances(typeof(IEventAggregator)).FirstOrDefault() as IEventAggregator;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            // Start camera
            ViewFinder.SensorLocation = CameraSensorLocation.Front;
            ViewFinder.Start();
        }

        public void Handle(CommandMessage message)
        {
            // Clear checked items
            if (message.Command == Commands.SwitchCamera)
            {
                ViewFinder.SensorLocation = ViewFinder.SensorLocation == CameraSensorLocation.Front ? CameraSensorLocation.Back : CameraSensorLocation.Front;
            }
        }
    }
}