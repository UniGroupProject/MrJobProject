using System;
using System.Windows.Data;

namespace MrJobProject.Converters
{
    /// <summary>
    ///  Klasa BoolToActiveConverter, wykorzystana przez interfejs graficzny do przetworzenia stanu True/False na brak napisu lub napis "Aktywny"
    /// </summary>
    /// <remarks>
    /// Klasa BoolToActiveConverter zawiera 2 metody
    /// </remarks>
    public class BoolToActiveConverter : IValueConverter
    {
        /// <summary>
        ///  Metoda Convert, przetwarza stan True/False na brak napisu lub napis "Aktywny"
        /// </summary>
        /// <returns>
        /// Zwraca pustego stringa dla "False" lub stringa "Aktywny" dla "True"
        /// </returns>
        /// <param name="value">Argument typu object, ktory przekazuje obiekt(wartosc) do przetworzenia</param>
        /// <param name="targetType">Argument typu Type, ktory przekazuje informacje o typie docelowym </param>
        /// <param name="parameter">Argument typu object, ktory przekazuje parametr</param>
        /// <param name="culture">Argument typu CultureInfo, ktory przekazuje informacje o jezyku docelowym </param>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true) return "Aktywny"; //language item
            }
            return "";
        }
        /// <summary>
        ///  Metoda ConvertBack, przetwarza string pusty na "False" lub string "Aktywny" na "True" 
        /// </summary>
        /// <returns>
        /// Zwraca pustego bool "False" dla pustego stringa lub "True" dla stringa "Aktywny"
        /// </returns>
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