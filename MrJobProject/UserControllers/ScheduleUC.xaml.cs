using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for ScheduleUC.xaml
    /// </summary>
    public partial class ScheduleUC : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Shift> shifts;
        private ObservableCollection<Worker> workers;
        private List<Holiday> holidays;
        private List<Schedule> schedules;

        public string[,] data2d;
        public int[] columnHeaders;

        public event PropertyChangedEventHandler PropertyChanged;

        public string[,] Data2D
        {
            get => this.data2d;
            private set
            {
                if (ReferenceEquals(value, this.data2d))
                {
                    return;
                }

                this.data2d = value;
                this.OnPropertyChanged();
            }
        }

        public int[] ColumnHeaders
        {
            get => this.columnHeaders;
            private set
            {
                if (ReferenceEquals(value, this.columnHeaders))
                {
                    return;
                }

                this.columnHeaders = value;
                this.OnPropertyChanged();
            }
        }

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

            UpdateLists();
            ReadScheduleList();
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

                int selectedYear = (int)ListOfYears.SelectedValue;
                int selectedMonth = (int)ListOfMonths.SelectedValue;
                connection.CreateTable<Holiday>(); //read all holidays in selected month/year
                holidays = new List<Holiday>
                    (connection.Table<Holiday>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear)));
            }
        }

        private void ReadScheduleList()
        {
            if (ListOfYears.SelectedValue != null && ListOfMonths.SelectedValue != null)
            {
                int daysOfMonth = DateTime.DaysInMonth((int)ListOfYears.SelectedValue, (int)ListOfMonths.SelectedValue); //

                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath)) // connect with database
                {
                    int selectedYear = (int)ListOfYears.SelectedValue;
                    int selectedMonth = (int)ListOfMonths.SelectedValue;

                    connection.CreateTable<Holiday>(); //read all holidays in selected month/year
                    holidays = new List<Holiday>
                        (connection.Table<Holiday>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear)));

                    connection.CreateTable<Schedule>(); //read all schedules in selected month/year
                    schedules = new List<Schedule>
                        (connection.Table<Schedule>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear)));
                }

                string[,] data = new string[workers.Count, daysOfMonth];
                for (int i = 0; i < workers.Count; i++)
                {
                    for (int j = 0; j < daysOfMonth; j++)
                    {
                        if (holidays.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).Count() > 0) // if any holiday then
                            data[i, j] = "U"; // remember to do: if holiday and added shift in the same day, remove shift
                        else if (schedules.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).Count() > 0) // if schedule then
                            data[i, j] = schedules.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).First().ShiftName;
                        else // if nothing added
                            data[i, j] = "";
                    }
                }
                Data2D = data;
                ColumnHeaders = Enumerable.Range(1, daysOfMonth).ToArray<int>();
            }
        }

        private void UpdateLists()
        {
            ReadDatabase();
            ShiftList.ItemsSource = shifts;
            WorkersList.ItemsSource = workers;
            ReadScheduleList();
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
                        UpdateLists();
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
                UpdateLists();
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
                    UpdateLists();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) // when UC loaded - updates data
        {
            UpdateLists();
            ReadScheduleList();
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
            ReadScheduleList();
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
            ReadScheduleList();
        }

        private void ShiftList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var cells = ScheduleList.SelectedCells.ToList();
            if (cells.Count() != 0)
            {
                var shift = (Shift)ShiftList.SelectedItem; // selected shift from list
                foreach (DataGridCellInfo item in cells)
                {
                    int col = item.Column.DisplayIndex;
                    var row = ScheduleList.Items.IndexOf(item.Item); // Gogus uratowal kod
                    if (data2d[row, col] != "U") // if there is no holiday in this day
                        data2d[row, col] = shift.ShiftName;
                }
                var firstCellCol = cells.Last().Column.DisplayIndex;
                var firstCellRow = ScheduleList.Items.IndexOf(cells.Last().Item);

                Data2D = (string[,])data2d.Clone();

                Keyboard.Focus(ScheduleList);
                ScheduleList.CurrentCell = new DataGridCellInfo(ScheduleList.Items[firstCellRow], ScheduleList.Columns[firstCellCol]);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListOfYears_SelectionChanged(object sender, SelectionChangedEventArgs e) // when year changed
        {
            ReadScheduleList();
        }

        private void ListOfMonths_SelectionChanged(object sender, SelectionChangedEventArgs e) // when month changed
        {
            ReadScheduleList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                int daysOfMonth = DateTime.DaysInMonth((int)ListOfYears.SelectedValue, (int)ListOfMonths.SelectedValue); //

                connection.CreateTable<Schedule>();

                for (int i = 0; i < workers.Count; i++)
                {
                    for (int j = 0; j < daysOfMonth; j++)
                    {
                        var savedSchedule = schedules.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1));

                        if (data2d[i, j] == "")
                        {
                            if (savedSchedule.Count() > 0)
                                connection.Delete(savedSchedule.First());
                        }
                        else if (data2d[i, j] == "U")
                        {
                            if (savedSchedule.Count() > 0)
                                connection.Delete(savedSchedule.First());
                        }
                        else
                        {
                            if (savedSchedule.Count() > 0)
                            {
                                savedSchedule.First().ShiftName = data2d[i, j];
                                connection.InsertOrReplace(savedSchedule.First());
                            }
                            else
                            {
                                Schedule schedule = new Schedule()
                                {
                                    WorkerId = workers.ElementAt(i).Id,
                                    Date = new DateTime((int)ListOfYears.SelectedValue, (int)ListOfMonths.SelectedValue, j + 1),
                                    ShiftName = data2d[i, j]
                                };

                                connection.Insert(schedule);
                            }
                        }

                        //if (holidays.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).Count() > 0) // if any holiday then
                        //    data[i, j] = "U"; // remember to do: if holiday and added shift in the same day, remove shift
                        //else if (schedules.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).Count() > 0) // if schedule then
                        //    data[i, j] = schedules.Where(c => (c.WorkerId == workers.ElementAt(i).Id) && (c.Date.Day == j + 1)).First().ShiftName;
                        //else // if nothing added
                        //    data[i, j] = "";
                    }
                }
            }
            UpdateLists();
        }

      

        private void ScheduleList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var cells = ScheduleList.SelectedCells.ToList();
                if (cells.Count() != 0)
                {
                    var shift = (Shift)ShiftList.SelectedItem; // selected shift from list
                    foreach (DataGridCellInfo item in cells)
                    {
                        int col = item.Column.DisplayIndex;
                        var row = ScheduleList.Items.IndexOf(item.Item); // Gogus uratowal kod
                        if (data2d[row, col] != "U") // if there is no holiday in this day
                            data2d[row, col] = "";
                    }
                    var firstCellCol = cells.Last().Column.DisplayIndex;
                    var firstCellRow = ScheduleList.Items.IndexOf(cells.Last().Item);

                    Data2D = (string[,])data2d.Clone();

                    Keyboard.Focus(ScheduleList);
                    ScheduleList.CurrentCell = new DataGridCellInfo(ScheduleList.Items[firstCellRow], ScheduleList.Columns[firstCellCol]);
                }
            }
        }
    }
}