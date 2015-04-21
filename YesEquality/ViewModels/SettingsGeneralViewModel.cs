using Caliburn.Micro;
using PropertyChanged;
using YesEquality.Helpers;

namespace YesEquality.ViewModels
{
    [ImplementPropertyChanged]
    public class SettingsGeneralViewModel : Screen
    {
        public bool ReminderOneEnabled { get; set; }
        public bool ReminderTwoEnabled { get; set; }
        
        public SettingsGeneralViewModel()
        {
            DisplayName = "general";
        }

        protected override void OnViewLoaded(object view)
        {
            ReminderOneEnabled = ReminderHelper.ReminderOneEnabled;
            ReminderTwoEnabled = ReminderHelper.ReminderTwoEnabled;
        }

        protected override void OnDeactivate(bool close)
        {
            ReminderHelper.ReminderOneEnabled = ReminderOneEnabled;
            ReminderHelper.ReminderTwoEnabled = ReminderTwoEnabled;
        }
    }
}
