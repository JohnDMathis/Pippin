using System;
using System.Globalization;
using System.Windows.Data;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2ImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Images/completed.jpg" : "Images/unfinished.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
