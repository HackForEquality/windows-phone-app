using Caliburn.Micro;

namespace YesEquality.ViewModels
{
    public class SettingsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        SettingsGeneralViewModel general;
        SettingsAboutViewModel about;

        public SettingsViewModel(SettingsGeneralViewModel general, SettingsAboutViewModel about)
        {
            this.general = general;
            this.about = about;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Setup pivot
            Items.Add(general);
            //Items.Add(about);
            ActivateItem(general);
        }
    }
}
