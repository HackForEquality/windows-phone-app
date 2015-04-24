using System;
using System.IO.IsolatedStorage;

namespace YesEquality.Helpers
{
    public static class SettingsHelper
    {
        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public static bool ShowBadgeTooltip
        {
            get
            {
                bool enabled;
                if (settings.TryGetValue("showBadgeTooltip", out enabled))
                {
                    return enabled;
                }
                return true;
            }
            set
            {
                settings["showBadgeTooltip"] = value;
                settings.Save();
            }
        }
    }
}