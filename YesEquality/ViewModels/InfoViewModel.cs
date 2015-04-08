using Caliburn.Micro;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using YesEquality.Models;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class InfoViewModel : Screen
    {
        public ObservableCollection<InfoModel> InfoList { get; set; }
        
        public InfoViewModel()
        {
            InfoList = new ObservableCollection<InfoModel>();

            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/1_Over18.png", UriKind.Relative), ImageText = "Over 18", BackgroundColour = "#7f4097" });
            InfoList.Add(new InfoModel() { Title = "To vote on May 22nd you must be:", ImagePath = new Uri("/Resources/Assets/Info/2_IrishCitizen.png", UriKind.Relative), ImageText = "Irish Citizen", BackgroundColour = "#1b75bb" });
        }
    }
}
