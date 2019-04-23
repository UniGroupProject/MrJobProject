using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;
using Gu.Wpf.DataGrid2D;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for ScheduleUC.xaml
    /// </summary>
    public partial class ScheduleUC : UserControl
    {
        ObservableCollection<Shift> shifts;
        ObservableCollection<Worker> workers;

        public string[,] data2d;
        public string[,] Data2D { get; set; }
        public int[] ColumnHeaders { get; set; }

        public ScheduleUC()
        {
            DataContext = this;
            InitializeComponent();


            shifts = new ObservableCollection<Shift>();
            workers = new ObservableCollection<Worker>();
            ListOfYears.ItemsSource = Enumerable.Range(2000, DateTime.Today.Year + 3 - 2000).ToList().Reverse<int>();
            ListOfMonths.ItemsSource = Enumerable.Range(1, 12).ToList();
            ListOfYears.SelectedValue = DateTime.Today.Year;
            ListOfMonths.SelectedValue = DateTime.Today.Month;

            UpdateList();
        }

        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>(); //workers list
                workers = new ObservableCollection<Worker>
                    (connection.Table<Worker>().Where(c => c.Status == true).ToList().OrderBy(c => c.Name).ToList().OrderByDescending(c => c.Status));

                connection.CreateTable<Shift>();
                shifts = new ObservableCollection<Shift> //shifts list
                    (connection.Table<Shift>().ToList().OrderBy(c => c.Id).ToList());
            }

            int daysOfMonth = DateTime.DaysInMonth((int)ListOfYears.SelectedValue, (int)ListOfMonths.SelectedValue); //put to uptadelist
            Data2D = new string[workers.Count, daysOfMonth];
            data2d = new string[workers.Count, daysOfMonth];
            for (int i = 0; i < workers.Count; i++)
            {
                for (int j = 0; j < daysOfMonth; j++)
                {
                    data2d[i, j] = "";
                }
            }
            Data2D = (string[,])data2d.Clone();

            ColumnHeaders = Enumerable.Range(1, daysOfMonth).ToArray<int>();
            BindingOperations.GetBindingExpression(ScheduleList, ItemsSource.Array2DProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(ScheduleList, ItemsSource.ColumnHeadersSourceProperty).UpdateTarget();
        }

        private void UpdateList()
        {
            ReadDatabase();
            ShiftList.ItemsSource = shifts;
            WorkersList.ItemsSource = workers;
        }

        private void DeleteShift(object sender, RoutedEventArgs e) // right click - delete shift
        {
            Shift shift = ShiftList.SelectedItem as Shift;
            if (shift != null)
            {
                string question = "Are you sure you want to delete " + shift.ShiftName + "?"; //language item
                YesNo Result = new YesNo(question);

                if (Result.ShowDialog() == true)
                {
                    if (ShiftList.SelectedItem != null)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                        {
                            connection.CreateTable<Shift>();
                            connection.Delete(ShiftList.SelectedItem);
                        }
                        UpdateList();
                    }
                }
            }
        }

        private void AddShiftDialog(object sender, RoutedEventArgs e) // right click - add new shift
        {
            EditAddShift addShift = new EditAddShift();
            if (addShift.ShowDialog() == true)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<Shift>();
                    connection.Insert(addShift.newShift);
                }
                UpdateList();
            }
        }

        private void EditShiftDialog(object sender, RoutedEventArgs e) // right click - edit selected shift
        {
            Shift shift = ShiftList.SelectedItem as Shift;
            if (shift != null)
            {
                EditAddShift editShift = new EditAddShift(shift);
                if (editShift.ShowDialog() == true)
                {
                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Shift>();
                        connection.InsertOrReplace(editShift.newShift);
                    }
                    UpdateList();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) // when UC loaded - updates data
        {
            UpdateList();
        }

        private void BackDateBtn_Click(object sender, RoutedEventArgs e) // button to select earlier month
        {
            int selectedYear = (int)ListOfYears.SelectedValue;
            int selectedMonth = (int)ListOfMonths.SelectedValue;
            if (selectedMonth == 1)
            {
                int size = ListOfYears.Items.Count;
                if (selectedYear == (int)ListOfYears.Items.GetItemAt(size - 1)) return;

                ListOfMonths.SelectedValue = 12;
                ListOfYears.SelectedValue = selectedYear - 1;
            }
            else
            {
                ListOfMonths.SelectedValue = selectedMonth - 1;
            }
            UpdateList();
        }

        private void ForwardDateBtn_Click(object sender, RoutedEventArgs e) // button to select forward month
        {
            int selectedYear = (int)ListOfYears.SelectedValue;
            int selectedMonth = (int)ListOfMonths.SelectedValue;
            if (selectedMonth == 12)
            {
                if (selectedYear == (int)ListOfYears.Items.GetItemAt(0)) return;
                ListOfMonths.SelectedValue = 1;
                ListOfYears.SelectedValue = selectedYear + 1;
            }
            else
            {
                ListOfMonths.SelectedValue = selectedMonth + 1;
            }
            ReadDatabase();
        }

        private void ShiftList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var cells = ScheduleList.SelectedCells.ToList();
            var shift = (Shift)ShiftList.SelectedItem;
            foreach (DataGridCellInfo item in cells)
            {
                int col = item.Column.DisplayIndex;
                var row = ScheduleList.Items.IndexOf(item.Item); // Gogus uratowal kod
                data2d[row, col] = shift.ShiftName;
            }
            Data2D = (string[,])data2d.Clone();
            BindingOperations.GetBindingExpression(ScheduleList, ItemsSource.Array2DProperty).UpdateTarget();
        }
    }
}
