using MrJobProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
