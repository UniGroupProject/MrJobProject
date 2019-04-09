using MrJobProject.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for WorkersUC.xaml
    /// </summary>
    public partial class WorkersUC : UserControl
    {
        ObservableCollection<Worker> workers;

        public WorkersUC()
        {
            InitializeComponent();

            workers = new ObservableCollection<Worker>();

            ReadDatabase();

            WorkersList.ItemsSource = workers;
        }

        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>();
                workers = new ObservableCollection<Worker>(connection.Table<Worker>().ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));
            }
        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {
            if (nameValue.Text != null)
            {
                Worker worker = new Worker()
                {
                    Name = nameValue.Text,
                    Status = statusValue.IsChecked.GetValueOrDefault()
                };
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<Worker>();
                    connection.Insert(worker);
                }
                ReadDatabase();
                WorkersList.ItemsSource = workers;
                nameValue.Text = "";
                statusValue.IsChecked = true;
            }
        }

        private void DatacontextItem_Delete(object sender, RoutedEventArgs e) //right click
        {
            Worker worker = WorkersList.SelectedItem as Worker;
            string question = "Are you sure you want to delete " + worker.Name; //language item
            MessageBoxResult Result = System.Windows.MessageBox.Show(question, "", System.Windows.MessageBoxButton.OKCancel);

            if (Result == MessageBoxResult.OK)
            {
                if (WorkersList.SelectedItem != null)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Worker>();
                        connection.Delete(WorkersList.SelectedItem);
                    }
                    ReadDatabase();
                    WorkersList.ItemsSource = workers;
                }
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) //enter
        {
            if (e.Key == Key.Return)
            {
                AddNewWorker(sender, null);
            }
        }

        private void EditWorker(object sender, MouseButtonEventArgs e) //double click
        {

        }
    }

    public class BoolToActiveConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is bool)
            {
                if ((bool)value == true) return "Active"; //language item

            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
