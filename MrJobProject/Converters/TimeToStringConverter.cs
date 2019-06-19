using System;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    /// <summary>
    ///  Klasa TimeToStringConverter, wykorzystana przez interfejs graficzny do przetworzenia zmiennej typu DateTime na string
    /// </summary>
    /// <remarks>
    /// Klasa TimeToStringConverter zawiera 2 metody
    /// </remarks>
    public class TimeToStringConverter : IValueConverter
    {
        /// <summary>
        ///  Metoda Convert, przetwarza zmienna typu DateTime i jej zawartosc na string z godzina
        /// </summary>
        /// /// <returns>
        /// Zwraca string z godzina
        /// </returns>
        /// <param name="value">Argument typu object, ktory przekazuje obiekt(wartosc) do przetworzenia</param>
        /// <param name="targetType">Argument typu Type, ktory przekazuje informacje o typie docelowym </param>
        /// <param name="parameter">Argument typu object, ktory przekazuje parametr</param>
        /// <param name="culture">Argument typu CultureInfo, ktory przekazuje informacje o jezyku docelowym </param>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime time = (DateTime)value;
                return time.ToString("H:mm");
            }
            return "";
        }

        /// <param name="value">Argument typu object, ktory przekazuje obiekt(wartosc) do przetworzenia</param>
        /// <param name="targetType">Argument typu Type, ktory przekazuje informacje o typie docelowym </param>
        /// <param name="parameter">Argument typu object, ktory przekazuje parametr</param>
        /// <param name="culture">Argument typu CultureInfo, ktory przekazuje informacje o jezyku docelowym </param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}