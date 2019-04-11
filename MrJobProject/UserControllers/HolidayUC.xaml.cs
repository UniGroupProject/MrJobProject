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
using MrJobProject;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for HolidayUC.xaml
    /// </summary>
    public partial class HolidayUC : UserControl
    {
        ObservableCollection<Worker> workers;

        public HolidayUC()
        {
            InitializeComponent();

            workers = new ObservableCollection<Worker>();

            UpdateList();
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

        private void DateMode(object sender, MouseButtonEventArgs e)//double click
        {
            var mainWindow = Application.Current.Windows[0] as MainWindow;
            mainWindow.ChangeToHolidayDateUC(WorkersList.SelectedItem as Worker);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
    }

    
}
