using System;
using System.Globalization;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    internal class NumberToMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int num = (int)value;
                string monthName = new DateTime(2000, num, 1).ToString("MMMMMMMMMMM", CultureInfo.CurrentCulture);//language item
                return monthName;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}