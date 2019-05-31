using System;
using System.Globalization;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    /// <summary>
    ///  Klasa NumberToMonthConverter, wykorzystana przez interfejs graficzny do przetworzenia numeru miesiaca na jego nazwe w jezyku polskim
    /// </summary>
    /// /// <remarks>
    /// Klasa NumberToMonthConverter zawiera 2 metody
    /// </remarks>
    public class NumberToMonthConverter : IValueConverter
    {
        /// <summary>
        ///  Metoda Convert, przetwarza liczbe z zakresu 1 do 12 na nazwe miesiaca
        /// </summary>
        /// /// <returns>
        /// Zwraca string z nazwa miesiaca
        /// </returns>
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

        /// <summary>
        ///  Metoda ConvertBack, przetwarza nazwe miesiaca na odpowiadajaca nazwe miesiaca
        /// </summary>
        /// <returns>
        /// Zwraca int odpowiadajacy podanej nazwie miesiaca
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}