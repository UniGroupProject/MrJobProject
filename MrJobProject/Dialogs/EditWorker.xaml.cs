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
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            newWorker.Name = nameValue.Text;
            newWorker.Status = (bool)statusValue.IsChecked;
            this.DialogResult = true;
        }
    }
}