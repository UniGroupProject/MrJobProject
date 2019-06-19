using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Logika okna dialogowego InfoOK
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor 
    /// </remarks>
    public partial class InfoOK : Window
    {
        public InfoOK(string text)
        {
            InitializeComponent();
            question.Text = text;
        }
        /// <summary>
        /// Metoda OKBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania zwraca prawde 
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}