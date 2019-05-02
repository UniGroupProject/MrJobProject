using MrJobProject.Data;
using MrJobProject.Dialogs;
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
    /// Interaction logic for ListsUC.xaml
    /// </summary>
    public partial class ListsUC : UserControl
    {

        ObservableCollection<Worker> workers;
        public ListsUC()
        {
            InitializeComponent();
            ListOfYears.ItemsSource = Enumerable.Range(2000, DateTime.Today.Year + 3 - 2000).ToList().Reverse<int>();
            ListOfMonths.ItemsSource = Enumerable.Range(1, 12).ToList();
            ListOfYears.SelectedValue = DateTime.Today.Year;
            ListOfMonths.SelectedValue = DateTime.Today.Month;

            workers = new ObservableCollection<Worker>();
            ReadDatabase();
        }
        private void ReadDatabase()
        {
            //TODO:
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>(); //workers list
                workers = new ObservableCollection<Worker>
                    (connection.Table<Worker>().Where(c => c.Status == true).ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));
                if (workers != null)
                {
                    workersListView.ItemsSource = workers;
                }
            }

        }
        private void ListOfYears_SelectionChanged(object sender, SelectionChangedEventArgs e) // when year changed
        {
            //TODO:
        }

        private void ListOfMonths_SelectionChanged(object sender, SelectionChangedEventArgs e) // when month changed
        {
            //TODO:
        }
    }
}
