using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    class NumberToMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                int num = (int)value;
                string monthName = new DateTime(2000, num, 1).ToString("MMMMMMMMMMM", CultureInfo.InvariantCulture);//language item
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
