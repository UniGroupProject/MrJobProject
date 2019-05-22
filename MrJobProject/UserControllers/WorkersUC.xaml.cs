using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for WorkersUC.xaml
    /// </summary>
    public partial class WorkersUC : UserControl
    {
        private ObservableCollection<Worker> workers;

        public WorkersUC()
        {
            InitializeComponent();

            workers = new ObservableCollection<Worker>();

            UpdateList();
        }

        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>();
                workers = new ObservableCollection<Worker>
                    (connection.Table<Worker>().ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));
            }
        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {
            if (nameValue.Text != "")
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
                UpdateList();
                nameValue.Text = "";
                statusValue.IsChecked = true;
            }
        }

        private void DatacontextItem_Delete(object sender, RoutedEventArgs e) //right click -> delete
        {
            Worker worker = WorkersList.SelectedItem as Worker;
            string question = "Czy jesteś pewien, że chcesz usunąć " + worker.Name + "?"; //language item
            YesNo Result = new YesNo(question);

            if (Result.ShowDialog() == true)
            {
                if (WorkersList.SelectedItem != null)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Worker>();
                        connection.Delete(WorkersList.SelectedItem);
                    }
                    UpdateList();
                }
            }
        }

        private void UpdateList()
        {
            ReadDatabase();
            WorkersList.ItemsSource = workers;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) //enter -c> add new worker
        {
            if (e.Key == Key.Return)
            {
                AddNewWorker(sender, null);
            }
        }

        private void EditWorker(object sender, MouseButtonEventArgs e) //double click -> edit worker
        {
            Worker selectedWorker = WorkersList.SelectedItem as Worker;
            EditWorker editWorker = new EditWorker(selectedWorker);
            if (editWorker.ShowDialog() == true)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<Worker>();
                    connection.InsertOrReplace(editWorker.newWorker);
                }
                UpdateList();
            }
        }
    }
}