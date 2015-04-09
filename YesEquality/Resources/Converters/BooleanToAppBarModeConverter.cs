using Microsoft.Phone.Shell;
using System;
using System.Windows.Data;

namespace YesEquality.Resources.Converters
{
    public class BooleanToAppBarModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? ApplicationBarMode.Default : ApplicationBarMode.Minimized;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
