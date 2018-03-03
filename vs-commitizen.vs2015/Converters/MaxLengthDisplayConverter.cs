using System;
using System.Globalization;
using System.Windows.Data;

namespace vs_commitizen.vs.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class MaxLengthDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value}/50";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
