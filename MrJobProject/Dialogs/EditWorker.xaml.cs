using MrJobProject.Data;
using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Interaction logic for EditWorker.xaml
    /// </summary>
    public partial class EditWorker : Window
    {
        public Worker newWorker;

        public EditWorker(Worker worker)
        {
            InitializeComponent();

            newWorker = worker;
            nameValue.Text = worker.Name;
            statusValue.IsChecked = worker.Status;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            newWorker.Name = nameValue.Text;
            newWorker.Status = (bool)statusValue.IsChecked;
            this.DialogResult = true;
        }
    }
}