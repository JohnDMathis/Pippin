using System;
using System.Globalization;
using System.Windows.Data;

namespace Odin.UI.Infrastructure.Converters
{
    public class Bool2ChangePwMsgMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "150,0,0,100";
            return "20,0,0,10";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
