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
            HolidayTypes.Add("Chorobowe");
            HolidayTypes.Add("Urlop");
            
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
        /// Metoda UserControl_Loaded(object sender, RoutedEventArgs e) powoduje wywolanie metody UpdateList()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }
        /// <summary>
        /// Metoda WorkersList_SelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// <param name="selectedWorker">Parametr selectedWorker przekazuje obiekt typu Worker, w tym przypadku wybranego pracownika</param>
        /// <param name="selectedYear">Parametr selectedYear przekazuje obiekt typu int, w tym przypadku wybranego roku</param>
        /// <param name="selectedMonth">Parametr selectedMonth przekazuje obiekt typu int, w tym przypadku wybranego miesiaca</param>
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
        /// Metoda ListOfYears_OnSelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ListOfYears_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }
        /// <summary>
        /// Metoda ListOfMonths_OnSelectionChanged(object sender, SelectionChangedEventArgs e) powoduje wywolanie metody SetCalendar()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ListOfMonths_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCalendar();
        }
        /// <summary>
        /// Metoda AddHolidayButton_OnClick(object sender, SelectionChangedEventArgs e) powoduje dodanie urlopu dla wybranego pracownika
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// Metoda DeleteHolidayButton_OnClick(object sender, RoutedEventArgs e) powoduje usuniecie urlopu danego pracownika
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// Metoda NoneButton_OnClick(object sender, RoutedEventArgs e) powoduje odznaczenie wszystkich dni na liscie
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void NoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectedItems.Clear();
        }

        /// <summary>
        /// Metoda AllButton_OnClick(object sender, RoutedEventArgs e) powoduje zaznaczenie wszystkich dni na liscie
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void AllButton_OnClick(object sender, RoutedEventArgs e)
        {
            HolidaysList.SelectAll();
        }
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) powoduje wywolanie metody UpdateList()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu DependencyPropertyChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl uc = sender as UserControl;
            if (uc.IsVisible)
            {
                UpdateList();
            }
        }

        private void WorkersList_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateList();
        }
    }
}