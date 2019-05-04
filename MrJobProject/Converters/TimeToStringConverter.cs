using System;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    internal class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime time = (DateTime)value;
                return ValueToString(time.Hour) + ":" + ValueToString(time.Minute);
            }
            return "";
        }

        private string ValueToString(int time)
        {
            if (time == 0) return "00";
            return time.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}