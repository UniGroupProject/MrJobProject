using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Logika kontrolki ScheduleUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, pola oraz zdarzenia
    /// </remarks>
    public partial class ScheduleUC : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// ObservableCollection  zawierajaca typ Shift o nazwie shifts zawiera liste zmian
        /// </summary>
        private ObservableCollection<Shift> shifts;
        /// <summary>
        /// ObservableCollection  zawierajaca typ Worker o nazwie workers zawiera liste pracownikow
        /// </summary>
        private ObservableCollection<Worker> workers;
        /// <summary>
        /// List  zawierajaca typ Holiday o nazwie holidays zawiera liste urlopów
        /// </summary>
        private List<Holiday> holidays;
        /// <summary>
        /// List  zawierajaca typ Schedule o nazwie schedules zawiera liste odpowiadajaca grafikowi
        /// </summary>
        private List<Schedule> schedules;
        /// <summary>
        /// tablica 2-wymiarowa zawierajaca typ string o nazwie data2d zawiera tabele odpowiadajaca grafikowi w interfejsie
        /// </summary>
        public string[,] data2d;
        /// <summary>
        /// tablica zawierajaca typ int o nazwie data2d zawiera liste nagłowkow do tabeli z grafikiem
        /// </summary>
        public int[] columnHeaders;
        /// <summary>
        /// Metoda obsugujace zmiane wlasciwosci
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private bool contentChanged = false;
        /// <value>Zwraca lub ustawia pole data2d</value>
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
        /// <value>Zwraca lub ustawia tablice columnHeaders</value>
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
        /// <summary>
        /// Metoda ReadDatabase() pobiera z bazy danych liste pracownikow i liste zmian
        /// </summary>
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
        /// <summary>
        /// Metoda ReadScheduleList() pobiera z bazy danych liste z grafikami
        /// </summary>
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
                            data[i, j] = "Z"; // remember to do: if holiday and added shift in the same day, remove shift
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
        /// <summary>
        /// Metoda UpdateLists() uaktualnia dane w UI
        /// </summary>
        private void UpdateLists()
        {
            ReadDatabase();
            ShiftList.ItemsSource = shifts;
            WorkersList.ItemsSource = workers;
            ReadScheduleList();
        }
        /// <summary>
        /// Metoda DeleteShift(object sender, TextChangedEventArgs e) podczas wywolania usuwa wybrana zmiane z listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void DeleteShift(object sender, RoutedEventArgs e) // right click - delete shift
        {
            Shift shift = ShiftList.SelectedItem as Shift;
            if (shift != null)
            {
                string question = "Czy jesteś pewien, że chcesz usunąć " + shift.ShiftName + "?"; //language item
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
        /// <summary>
        /// Metoda AddShiftDialog(object sender, TextChangedEventArgs e) podczas wywolania dodaje zmiane do listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// <summary>
        /// Metoda EditShiftDialog(object sender, TextChangedEventArgs e) podczas wywolania edytuje wybrana zmiane z listy i bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
        /// <summary>
        /// Metoda BackDateBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania cofa wybrana date o miasiac
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void BackDateBtn_Click(object sender, RoutedEventArgs e) // button to select earlier month
        {
            AskAboutSaving();
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
        /// <summary>
        /// Metoda ForwardDateBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania zmienia wybrana date o miasiac wprzod
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ForwardDateBtn_Click(object sender, RoutedEventArgs e) // button to select forward month
        {
            AskAboutSaving();
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
        /// <summary>
        /// Metoda ShiftList_PreviewMouseLeftButtonUp(object sender, TextChangedEventArgs e) podczas wywolania zmienia zawartosc wybranej komorki w tabeli z grafikiem na nazwe wybranego elementu z listy zmian
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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
                    if (data2d[row, col] != "Z") // if there is no holiday in this day
                        data2d[row, col] = shift.ShiftName;
                }
                var firstCellCol = cells.Last().Column.DisplayIndex;
                var firstCellRow = ScheduleList.Items.IndexOf(cells.Last().Item);

                Data2D = (string[,])data2d.Clone();

                Keyboard.Focus(ScheduleList);
                ScheduleList.CurrentCell = new DataGridCellInfo(ScheduleList.Items[firstCellRow], ScheduleList.Columns[firstCellCol]);
                contentChanged = true;
            }
        }
        /// <summary>
        /// Metoda OnPropertyChanged() wzbudza zdarzanie, gdy pewna wlasciwosc jest zmieniona
        /// </summary>
        /// /// <param name="propertyName">Argument typu string, ktory przekazuje nazwe wlasciwosci, jesli nie jest przekazany do metody, to bazowo ustawiony jest null</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Metoda ListOfYears_SelectionChanged(object sender, TextChangedEventArgs e) podczas wywolania uaktulania tabele z grafikiem
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu SelectionChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ListOfYears_SelectionChanged(object sender, SelectionChangedEventArgs e) // when year changed
        {
            AskAboutSaving();
            ReadScheduleList();
        }
        /// <summary>
        /// Metoda ListOfMonths_SelectionChanged(object sender, TextChangedEventArgs e) podczas wywolania uaktulania tabele z grafikiem
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu SelectionChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ListOfMonths_SelectionChanged(object sender, SelectionChangedEventArgs e) // when month changed
        {
            AskAboutSaving();
            ReadScheduleList();
        }
        /// <summary>
        /// Metoda SaveBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania zapisuje dane z tabeli z grafikiem do bazy danych
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e) // save button cklicked
        {
            SaveChanges();
        }

        private void SaveChanges()
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
                        else if (data2d[i, j] == "Z")
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
                    }
                }
            }
            UpdateLists();
            contentChanged = false;
        }

        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, TextChangedEventArgs e) podczas wywolania uaktualnia dane po powrocie z innych zakladek
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu DependencyPropertyChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) //when UserConrol loaded -> update
        {
            UserControl uc = sender as UserControl;
            if (uc.IsVisible)
            {
                UpdateLists();
                ReadScheduleList();
            }
            else if(!uc.IsVisible)
            {
                AskAboutSaving();
            }
        }
        /// <summary>
        /// Metoda ScheduleList_PreviewKeyDown(object sender, KeyEventArgs e), ktora zostaje wywolana po nacisnieciu przycisku delete, usuwa zawartosc zaznaczonych komorek
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu KeyEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void ScheduleList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var cells = ScheduleList.SelectedCells.ToList();
                foreach (DataGridCellInfo item in cells)
                {
                    int col = item.Column.DisplayIndex;
                    var row = ScheduleList.Items.IndexOf(item.Item);

                    data2d[row, col] = "";
                }
                Data2D = (string[,])data2d.Clone();
                contentChanged = true;
            }
        }

        private void RestoreDB(string filename)
        {
            var srcfile = Path.GetFullPath(filename);
            var destfile = Path.GetFullPath(App.databasePath);

            if (File.Exists(destfile)) File.Delete(destfile);

            File.Copy(srcfile, destfile);
            UpdateLists();

            //todo:  zmiana nazwy pliku doceloego na ADB
        }

        private void BackUpBtn_Click(object sender, RoutedEventArgs e)
        {
            Backup();
        }

        private void Backup()
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = folderDialog.ShowDialog();
            string folderPath = "";
            if (result == WinForms.DialogResult.OK)
            {
                //----< Selected Folder >---- 
                //< Selected Path > 
                String sPath = folderDialog.SelectedPath;
                folderPath = sPath;
                //</ Selected Path >
            }
            else return;
            var srcfile = Path.GetFullPath(App.databasePath);
            var destfile = Path.Combine(folderPath + "\\Backup" + DateTime.Today.ToString("yyyy-MM-dd") +"_"+DateTime.Now.ToString("HH-mm")+ ".bak");

            if (File.Exists(destfile)) File.Delete(destfile);

            File.Copy(srcfile, destfile);

            UpdateLists();
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            //ask about backup
            string question = "Czy chcesz utworzyć kopię zapasową aktualnej wersji?"; //language item
            YesNo Result = new YesNo(question);

            if (Result.ShowDialog() == true)
            {
                Backup();
            }


            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".bak"; // Default file extension
            dlg.Filter = "Backup files (.bak)|*.bak"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                RestoreDB(filename);
                UpdateLists();
            }
        }

        private void AskAboutSaving()
        {
            string question = "Czy chcesz zapisać zmiany?"; //language item
            YesNo Result = new YesNo(question);
            if (contentChanged)
            {
                if (Result.ShowDialog() == true)
                {
                    SaveChanges();
                }
                else
                    contentChanged = false;
            }
        }
    }
}