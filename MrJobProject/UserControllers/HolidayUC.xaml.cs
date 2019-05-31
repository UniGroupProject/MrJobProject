using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MrJobProject.Classes;
using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Logika kontrolki HolidayUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, pola, metody oraz zdarzenia
    /// </remarks>
    public partial class HolidayUC : UserControl
    {
        /// <summary>
        /// ObservableCollection zawierajaca typ Worker o nazwie workers zawiera liste pracownikow
        /// </summary>
        private ObservableCollection<Worker> workers;

        /// <summary>
        /// ObservableCollection zawierajaca typ Holiday o nazwie holidays zawiera liste urlopow
        /// </summary>
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

        /// <summary>
        /// Metoda GetListOfHolidayTypes(), ktora zwraca typy urlopow
        /// </summary>
        /// <returns>
        ///  Zwraca liste string-ow, ktora zawiera typy urlopow
        /// </returns>
        private List<string> GetListOfHolidayTypes()
        {
            var HolidayTypes = new List<string>(); //language item //change to eng version
            HolidayTypes.Add("Choroba pracownika (płaci pracodawca)");
            HolidayTypes.Add("Choroba pracownika (płaci ZUS)");
            HolidayTypes.Add("Choroba niepłatne");
            HolidayTypes.Add("Opieka");
            HolidayTypes.Add("Wypoczynkowy");
            HolidayTypes.Add("Na żądanie");
            HolidayTypes.Add("Bezpłatny");
            HolidayTypes.Add("Okolicznościowy");
            HolidayTypes.Add("Macierzyński");
            HolidayTypes.Add("Wychowawczy");
            HolidayTypes.Add("Nieobecność usprawiedliwiona bezpłatna");
            HolidayTypes.Add("Inne obecności nieusprawiediwione");
            return HolidayTypes;
        }
        /// <summary>
        /// Metoda UpdateList() aktualizuje liste pracownikow
        /// </summary>
        private void UpdateList()
        {
            ReadDatabase();
            WorkersList.ItemsSource = workers;
        }
        /// <summary>
        /// Metoda ReadDatabase() pobiera liste pracownikow z bazy danych
        /// </summary>
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
        /// <summary>
        /// Zdarzenie UserControl_Loaded(object sender, RoutedEventArgs e) powoduje wywolanie metody UpdateList()
        /// </summary>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
        /// <summary>
        /// Zdarzenie WorkersList_SelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        private void WorkersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }
        /// <summary>
        /// Metoda SetCalendar() powoduje wczytanie kalendarza do ustawiania urlopow wybranego pracownika
        /// </summary>
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
        /// <summary>
        /// Metoda ReadHolidaysDatabase(Worker selectedWorker, int selectedYear, int selectedMonth) powoduje pobranie z bazy danych dni z urlopami dla danego pracownika, w okreslonym roku i miesiacu
        /// </summary>
        /// <param name="selectedWorker"></param>
        /// <param name="selectedYear"></param>
        /// <param name="selectedMonth"></param>
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
        /// <summary>
        /// Zdarzenie ListOfYears_OnSelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        private void ListOfYears_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }
        /// <summary>
        /// Zdarzenie ListOfMonths_OnSelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        private void ListOfMonths_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }
        /// <summary>
        /// Zdarzenie AddHolidayButton_OnClick(object sender, SelectionChangedEventArgs e) powoduje dodanie urlopu dla wybranego pracownika
        /// </summary>
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

            InfoOK info = new InfoOK("Dodano urlop");
            info.ShowDialog();
        }
        /// <summary>
        /// Zdarzenie DeleteHolidayButton_OnClick(object sender, RoutedEventArgs e) powoduje usuniecie urlopu danego pracownika
        /// </summary>
        private void DeleteHolidayButton_OnClick(object sender, RoutedEventArgs e)
        {
            string question = "Czy jesteś pewien, że chcesz usunąć?"; //language item
            YesNo Result = new YesNo(question);

            if (Result.ShowDialog() == true)
            {
                var selectedWorker = (Worker)WorkersList.SelectedItem;
                var selectedMonth = (int)ListOfMonths.SelectedValue;
                var selectedYear = (int)ListOfYears.SelectedValue;

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
        }

        /// <summary>
        /// Zdarzenie NoneButton_OnClick(object sender, RoutedEventArgs e) powoduje odznaczenie wszystkich dni na liscie
        /// </summary>
        private void NoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectedItems.Clear();
        }

        /// <summary>
        /// Zdarzenie AllButton_OnClick(object sender, RoutedEventArgs e) powoduje zaznaczenie wszystkich dni na liscie
        /// </summary>
        private void AllButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectAll();
        }
        /// <summary>
        /// Zdarzenie UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) powoduje wywolanie metody UpdateList()
        /// </summary>
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