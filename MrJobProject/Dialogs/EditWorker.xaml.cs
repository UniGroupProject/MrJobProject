using MrJobProject.Data;
using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Logika okna dialogowego EditWorker
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor oraz pole
    /// </remarks>
    public partial class EditWorker : Window
    {
        /// <summary>
        /// zmienna typu Worker o nazwie newWorker informacje o nowym pracowniku
        /// </summary>
        public Worker newWorker;

        public EditWorker(Worker worker)
        {
            InitializeComponent();

            newWorker = worker;
            nameValue.Text = worker.Name;
            statusValue.IsChecked = worker.Status;
        }
        /// <summary>
        /// Metoda SaveBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania edytuje wybranego pracownika i zapisuje w bazie danych 
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            newWorker.Name = nameValue.Text;
            newWorker.Status = (bool)statusValue.IsChecked;
            this.DialogResult = true;
        }
    }
}