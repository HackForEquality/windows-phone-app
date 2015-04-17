using Caliburn.Micro;
using Microsoft.Phone.Tasks;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using YesEquality.Models;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class InfoViewModel : Screen, IHandle<WebBrowserTask>
    {
        private readonly IEventAggregator eventAggregator;
        private readonly INavigationService navigationService;
        public ObservableCollection<InfoModel> InfoList { get; set; }
        public bool ExpandAppBar { get; set; }

        public InfoViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            InfoList = new ObservableCollection<InfoModel>();
            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/1_Over18.png", UriKind.Relative), ImageText = "Over 18", BackgroundColour = "#7f4097" });
            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/2_IrishCitizen.png", UriKind.Relative), ImageText = "Irish Citizen", BackgroundColour = "#1b75bb" });
            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/3_Resident.png", UriKind.Relative), ImageText = "Resident in the republic", BackgroundColour = "#2e358e" });
            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/4_Registered.png", UriKind.Relative), ImageText = "Registered to vote", BackgroundColour = "#9e2064" });
            InfoList.Add(new InfoModel() { Title = "Check to see if you are registered", ImagePath = new Uri("/Resources/Assets/Info/5_Check.png", UriKind.Relative), ImageText = "checktheregister.ie", BackgroundColour = "#292561" });
            InfoList.Add(new InfoModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/6_Download.png", UriKind.Relative), ImageText = "Download the form", BackgroundColour = "#009348" });
            InfoList.Add(new InfoModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/7_Fillin.png", UriKind.Relative), ImageText = "Get it signed & stamped by a Garda", BackgroundColour = "#91288c" });
            InfoList.Add(new InfoModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/8_PostForms.png", UriKind.Relative), ImageText = "Return it to your local authority post office", BackgroundColour = "#be202e" });
            InfoList.Add(new InfoModel() { Title = "Student?", ImagePath = new Uri("/Resources/Assets/Info/9_Student.png", UriKind.Relative), ImageText = "You can vote by post", BackgroundColour = "#7bc043" });
            InfoList.Add(new InfoModel() { Title = "Away at work?", ImagePath = new Uri("/Resources/Assets/Info/10_AtWork.png", UriKind.Relative), ImageText = "You can vote by post too!", BackgroundColour = "#7f4097" });
            InfoList.Add(new InfoModel() { Title = "Changed address?", ImagePath = new Uri("/Resources/Assets/Info/11_Address.png", UriKind.Relative), ImageText = "You can vote by post", BackgroundColour = "#1b75bb" });

            ExpandAppBar = false;
        }

        #region Commands
        public void SlideSelectionChanged(SelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0] as InfoModel;

            // Only show browser button when on a certain slide
            if (item.ImageText.Contains("checktheregister"))
            {
                ExpandAppBar = true;
            }
            else
            {
                ExpandAppBar = false;
            }
        }
        public void PaginationLoaded(RadPaginationControl pagination)
        {
            // Show pagination control after loading
            // Fix for 'No Data to display' message
            pagination.Visibility = Visibility.Visible;
        }

        public void GoToWebsite()
        {
            eventAggregator.RequestTask<WebBrowserTask>(x => { x.Uri = new Uri("https://www.checktheregister.ie/"); });
        }

        public void Handle(WebBrowserTask message)
        {
        }
        #endregion
    }
}
