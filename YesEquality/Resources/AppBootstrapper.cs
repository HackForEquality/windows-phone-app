using System;
using System.Collections.Generic;
using Caliburn.Micro;
using System.Windows;
using System.Diagnostics;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls;
using YesEquality.ViewModels;
using System.Windows.Navigation;

namespace YesEquality
{
    public class AppBootstrapper : PhoneBootstrapperBase
    {
        public PhoneContainer container { get; set; }
        private PhoneApplicationFrame rootFrame;
        private bool reset;

        public AppBootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {
            container = new PhoneContainer();
            if (!Execute.InDesignMode)
            {
                container.RegisterPhoneServices(RootFrame);
            }

            // Hook navigation events for Fast App Resume
            rootFrame.Navigated += rootFrame_Navigated;
            rootFrame.Navigating += rootFrame_Navigating;

            // Define VMs
            container.PerRequest<MainViewModel>();
            
            // Add custom binding conventions
            AddCustomConventions();
        }

        void rootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (reset && e.IsCancelable && e.Uri.OriginalString.Contains("MainView.xaml"))
            {
                e.Cancel = true;
                reset = false;
            }
        }

        void rootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            reset = e.NavigationMode == NavigationMode.Reset;
        }

        protected override PhoneApplicationFrame CreatePhoneApplicationFrame()
        {
            rootFrame = new PhoneApplicationFrame();
            return rootFrame;
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
        }

        protected override void OnLaunch(object sender, LaunchingEventArgs e)
        {
        }

        protected override void OnActivate(object sender, ActivatedEventArgs e)
        {
        }

        protected override void OnDeactivate(object sender, DeactivatedEventArgs e)
        {
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debug.WriteLine(e.ExceptionObject.Message);
                Debugger.Break();
            }
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        static void AddCustomConventions()
        {

        }
    }
}
