using Caliburn.Micro;
using PropertyChanged;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class SettingsAboutViewModel : Screen
    {
        public string AppVersion { get; set; }

        public SettingsAboutViewModel()
        {
            DisplayName = "about";
            AppVersion = "v1.0.1";
        }
    }
}
