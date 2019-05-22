using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MrJobProject.Classes;
using MrJobProject.Data;
using SQLite;

namespace MrJobProject.UserControllers
{
    /// <summary>
    ///     Interaction logic for HolidayUC.xaml
    /// </summary>
    public partial class HolidayUC : UserControl
    {
        private ObservableCollection<Worker> workers;
        private ObservableCollection<Holiday> holidays;


        public HolidayUC()
        {
            InitializeComponent();

            ListOfYears.ItemsSource = Enumerable.Range(2000, DateTime.Today.Year + 3 - 2000).ToList().Reverse<int>();
            ListOfMonths.ItemsSource = Enumerable.Range(1, 12).ToList();
            ListOfReasons.ItemsSource = GetListOfHolidayTypes();

            ListOfYears.SelectedValue = DateTime.Today.Year;
            ListOfMonths.SelectedValue = DateTime.Today.Month;
            ListOfReasons.SelectedValue = GetListOfHolidayTypes().First();


            workers = new ObservableCollection<Worker>();

            UpdateList();
        }

        private List<string> GetListOfHolidayTypes()
        {
            var HolidayTypes = new List<string>(); //language item //change to eng version
            HolidayTypes.Add("choroba pracownika (płaci pracodawca)");
            HolidayTypes.Add("choroba pracownika (płaci ZUS)");
            HolidayTypes.Add("choroba niepłatne");
            HolidayTypes.Add("opieka");
            HolidayTypes.Add("wypoczynkowy");
            HolidayTypes.Add("na żadanie");
            HolidayTypes.Add("bezpłatny");
            HolidayTypes.Add("okolicznościowy");
            HolidayTypes.Add("macierzyński");
            HolidayTypes.Add("wychowawczy");
            HolidayTypes.Add("nieobecność usprawiedliwiona bezpłatna");
            HolidayTypes.Add("inne obecności nieusprawiediwione");
            return HolidayTypes;
        }

        private void UpdateList()
        {
            ReadDatabase();
            WorkersList.ItemsSource = workers;
        }

        private void ReadDatabase()
        {
            using (var connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Worker>();
                workers = new ObservableCollection<Worker>
                (connection.Table<Worker>().ToList().OrderBy(c => c.Name).ToList()
                    .OrderByDescending(c => c.Status));
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void WorkersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }

        private void SetCalendar()
        {
            var selectedWorker = (Worker) WorkersList.SelectedItem;
            if (selectedWorker != null)
            {
                var selectedMonth = (int) ListOfMonths.SelectedValue;
                var selectedYear = (int) ListOfYears.SelectedValue;
                var dayList = new ObservableCollection<Day>();


                ReadHolidaysDatabase(selectedWorker, selectedYear, selectedMonth);

                for (var day = 1; day <= DateTime.DaysInMonth(selectedYear, selectedMonth); day++)
                {
                    if (holidays.Where(c => c.Date.Day == day).Count() == 0)
                    {
                        dayList.Add(new Day(day, false));
                    }
                    else
                    {
                        var reason = holidays.Where(c => c.Date.Day == day).First().Type;

                        dayList.Add(new Day(day, true, reason));
                    }

                    HolidaysList.ItemsSource = dayList;
                }
            }
        }

        private void ReadHolidaysDatabase(Worker selectedWorker, int selectedYear, int selectedMonth)
        {
            using (var connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Holiday>();
                holidays = new ObservableCollection<Holiday>
                (connection.Table<Holiday>().ToList().Where(c =>
                        c.WorkerId == selectedWorker.Id && c.Date.Year == selectedYear && c.Date.Month == selectedMonth)
                    .ToList());
            }
        }

        private void ListOfYears_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }

        private void ListOfMonths_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }

        private void AddHolidayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedWorker = (Worker) WorkersList.SelectedItem;
            var selectedMonth = (int) ListOfMonths.SelectedValue;
            var selectedYear = (int) ListOfYears.SelectedValue;
            var selectedReason = (string) ListOfReasons.SelectedValue;
            var selectedDays = HolidaysList.SelectedItems.Cast<Day>().ToList();

            using (var connection = new SQLiteConnection(App.databasePath))
            {
                foreach (var selectedDay in selectedDays)
                    if (!selectedDay.IsHoliday)
                    {
                        var holiday = new Holiday
                        {
                            WorkerId = selectedWorker.Id,
                            Date = new DateTime(selectedYear, selectedMonth, selectedDay.DayNumber),
                            Type = selectedReason
                        };

                        connection.Insert(holiday);
                    }
                    else if (selectedDay.IsHoliday && selectedDay.Reason != selectedReason)
                    {
                        var holiday = new Holiday
                        {
                            WorkerId = selectedWorker.Id,
                            Date = new DateTime(selectedYear, selectedMonth, selectedDay.DayNumber),
                            Type = selectedReason
                        };

                        connection.InsertOrReplace(holiday);
                        ;
                    }
            }

            SetCalendar();
        }

        private void DeleteHolidayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedWorker = (Worker) WorkersList.SelectedItem;
            var selectedMonth = (int) ListOfMonths.SelectedValue;
            var selectedYear = (int) ListOfYears.SelectedValue;

            var selectedDays = HolidaysList.SelectedItems.Cast<Day>().ToList();

            using (var connection = new SQLiteConnection(App.databasePath))
            {
                foreach (var selectedDay in selectedDays)
                    if (selectedDay.IsHoliday)
                    {
                        var holiday = holidays.Where(c => c.Date.Day.ToString() == selectedDay.DayNumber.ToString())
                            .ToList().First();

                        connection.Delete(holiday);
                    }
            }


            SetCalendar();
        }


        private void NoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectedItems.Clear();
        }

        private void AllButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectAll();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl uc = sender as UserControl;
            if (uc.IsVisible)
            {
                UpdateList();
            }
        }
    }
}