using System;
using System.Windows.Data;

namespace Pippin.VMLocator
{
    public class IndexerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var locator = (ViewModelLocator)value;
            string index = parameter.ToString();
            return locator.Find[index];
            //return locator.FindShared[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}