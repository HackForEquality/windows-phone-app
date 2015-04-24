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
        public ObservableCollection<SlideModel> SlideList { get; set; }

        public InfoViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            SlideList = new ObservableCollection<SlideModel>();
            SlideList.Add(new SlideModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/1_Over18.png", UriKind.Relative), ImageText = "Over 18", BackgroundColour = "#7f4097" });
            SlideList.Add(new SlideModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/2_IrishCitizen.png", UriKind.Relative), ImageText = "Irish Citizen", BackgroundColour = "#1b75bb" });
            SlideList.Add(new SlideModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/3_Resident.png", UriKind.Relative), ImageText = "Resident in the republic", BackgroundColour = "#2e358e" });
            SlideList.Add(new SlideModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/4_Registered.png", UriKind.Relative), ImageText = "Registered to vote", BackgroundColour = "#9e2064" });
            SlideList.Add(new SlideModel() { Title = "Check to see if you are registered", ImagePath = new Uri("/Resources/Assets/Info/5_Check.png", UriKind.Relative), ImageText = "checktheregister.ie", BackgroundColour = "#292561", Type = SlideModelType.HasLink });
            SlideList.Add(new SlideModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/6_Download.png", UriKind.Relative), ImageText = "Download the form", BackgroundColour = "#009348", Type = SlideModelType.HasLink });
            SlideList.Add(new SlideModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/7_Fillin.png", UriKind.Relative), ImageText = "Get it signed & stamped by a Garda", BackgroundColour = "#91288c" });
            SlideList.Add(new SlideModel() { Title = "Registering is easy", ImagePath = new Uri("/Resources/Assets/Info/8_PostForms.png", UriKind.Relative), ImageText = "Return it to your local authority office by May 5th", BackgroundColour = "#be202e" });
            SlideList.Add(new SlideModel() { Title = "On May 22nd", ImagePath = new Uri("/Resources/Assets/Info/9_Student.png", UriKind.Relative), ImageText = "Polling stations will be open from 7am to 10pm.", BackgroundColour = "#7bc043" });
            //SlideList.Add(new InfoModel() { Title = "Away at work?", ImagePath = new Uri("/Resources/Assets/Info/10_AtWork.png", UriKind.Relative), ImageText = "You can vote by post too!", BackgroundColour = "#7f4097" });
            //SlideList.Add(new InfoModel() { Title = "Changed address?", ImagePath = new Uri("/Resources/Assets/Info/11_Address.png", UriKind.Relative), ImageText = "You can vote by post", BackgroundColour = "#1b75bb" });
            SlideList.Add(new SlideModel() { Title = "And remember...", ImagePath = new Uri("/Resources/Assets/CampaignLogo.png", UriKind.Relative), ImageText = "Your vote counts. Don't forget to use it.", BackgroundColour = "#ffffff", TextColour = "Black" });
        }

        protected override void OnViewReady(object view)
        {
        }

        #region Commands
        public void PaginationLoaded(RadPaginationControl pagination)
        {
            // Show pagination control after loading
            // Fix for 'No Data to display' message
            pagination.Visibility = Visibility.Visible;
        }

        public void OpenLink(SlideModel slide)
        {
            if (slide.ImageText.Contains("checktheregister"))
            {
                eventAggregator.RequestTask<WebBrowserTask>(x => { x.Uri = new Uri("https://www.checktheregister.ie/"); });
            }
            else
            {
                eventAggregator.RequestTask<WebBrowserTask>(x => { x.Uri = new Uri("http://www.checktheregister.ie/appforms/RFA_English_Form.pdf"); });
            }
        }
        
        public void Handle(WebBrowserTask message)
        {
        }
        #endregion
    }
}
