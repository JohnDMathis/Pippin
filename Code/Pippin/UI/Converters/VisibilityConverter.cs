using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Pippin.UI.Converters
{
    public class VisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool not = parameter != null && (parameter.ToString() == "Not" | parameter.ToString() == "!");
            bool vis = (bool) value;
            if (not) vis = !vis;
            return vis ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = (Visibility) value;
            bool not = (parameter.ToString() == "Not" | parameter.ToString() == "!");
            bool result = (vis == Visibility.Visible);
            if (not) result = !result;
            return result;
        }
    }
}