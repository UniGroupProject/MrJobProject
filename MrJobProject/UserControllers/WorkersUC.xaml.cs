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
    /// Logika kontrolki WorkersUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, pola oraz zdarzenia
    /// </remarks>
    public partial class WorkersUC : UserControl
    {
        /// <summary>
        /// ObservableCollection  zawierajaca typ Worker o nazwie workers zawiera liste pracownikow
        /// </summary>
        private ObservableCollection<Worker> workers;

        public WorkersUC()
        {
            InitializeComponent();

            workers = new ObservableCollection<Worker>();

            UpdateList();
        }
        /// <summary>
        /// Metoda ReadDatabase() pobiera z bazy danych liste pracownikow
        /// </summary>
        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>();
                workers = new ObservableCollection<Worker>
                    (connection.Table<Worker>().ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));
            }
        }
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, TextChangedEventArgs e) podczas wywolania dodaje nowego pracownika do listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, TextChangedEventArgs e) podczas wywolania usuwa wybranego pracownika do listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// <summary>
        /// Metoda UpdateLists() uaktualnia dane w UI
        /// </summary>
        private void UpdateList()
        {
            ReadDatabase();
            WorkersList.ItemsSource = workers;
        }
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, TextChangedEventArgs e) podczas wywolania (wcisniecie enter) dodaje nowego pracownika do listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu KeyEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e) //enter -c> add new worker
        {
            if (e.Key == Key.Return)
            {
                AddNewWorker(sender, null);
            }
        }
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, TextChangedEventArgs e) podczas wywolania edytuje wybranego pracownika do listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu MouseButtonEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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