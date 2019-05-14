using MrJobProject.Data;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for HolidayUC.xaml
    /// </summary>
    public partial class HolidayUC : UserControl
    {
        private ObservableCollection<Worker> workers;

        public HolidayUC()
        {
            InitializeComponent();

            workers = new ObservableCollection<Worker>();

            UpdateList();
        }

        private void LoadBlackoutHolidays(Worker worker)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Holiday>();
                var holidays = connection.Table<Holiday>().ToList().Where(c => (c.WorkerId == worker.Id)).ToList();
                highlightCalendar.BlackoutDates.Clear();
                foreach (var worker_holiday in holidays)
                {
                    highlightCalendar.BlackoutDates.Add(
                         new CalendarDateRange(worker_holiday.Date, worker_holiday.Date)); //locks all the holiday days
                }
            }
        }

        private void UpdateList()
        {
            ReadDatabase();
            WorkersList.ItemsSource = workers;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void WorkersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Worker selectedWorker = (Worker)WorkersList.SelectedItem;
            if (selectedWorker != null)
            {
                LoadBlackoutHolidays(selectedWorker);
            }
        }
    }
}