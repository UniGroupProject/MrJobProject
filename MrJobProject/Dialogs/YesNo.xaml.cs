using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Logika okna dialogowego YesNo
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor 
    /// </remarks>
    public partial class YesNo : Window
    {
        public YesNo(string text)
        {
            InitializeComponent();
            question.Text = text;
        }
        /// <summary>
        /// Metoda YesBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania zwraca prawde 
        /// </summary>
        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}