using MrJobProject.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using MrJobProject.Dialogs;
using System.IO;


namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Logika kontrolki ListsUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, pola oraz zdarzenia
    /// </remarks>
    public partial class ListsUC : UserControl
    {   /// <summary>
        /// ObservableCollection  zawierajaca typ Worker o nazwie workers zawiera liste pracownikow
        /// </summary>
        private ObservableCollection<Worker> workers;
        /// <summary>
        /// List zawierajaca typ Worker o nazwie selectedWorkers zawiera liste wybranych pracownikow
        /// </summary>
        private List<Worker> selectedWorkers;


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
        /// <summary>
        /// Metoda ReadDatabase() pobiera z baz ydanych liste pracownikow
        /// </summary>
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
        /// <summary>
        /// Metoda SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) podczas wywolania filtruje liste pracownikow
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            var filteredList = workers.Where(c => c.Name.ToLower().Contains(searchTextBox.Text)).ToList();

            workersListView.ItemsSource = filteredList;
        }
        /// <summary>
        /// Metoda WorkersListView_SelectionChanged(object sender, SelectionChangedEventArgs e) podczas wywolania przypisuje do pola selectedWorkers zaznaczonych pracownikow z WorkersListView
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void WorkersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedWorkers = workersListView.SelectedItems.Cast<Worker>().ToList();
        }
        /// <summary>
        /// Metoda NoneButton_Click(object sender, RoutedEventArgs e) podczas wywolania odznacza wszystkich pracownikow z workersListView
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in workers)
            {
                workersListView.SelectedItems.Clear();
            }
            Keyboard.Focus(workersListView);
        }

        /// <summary>
        /// Metoda AllButton_Click(object sender, RoutedEventArgs e) podczas wywolania zaznacza wszystkich pracownikow z workersListView
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            workersListView.SelectAll();
        }
        /// <summary>
        /// Metoda PdfButton_OnClick(object sender, RoutedEventArgs e) podczas wywolania generuje liste obecnosci dla wybranych pracownikow w postaci plikow PDF
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void PdfButton_OnClick(object sender, RoutedEventArgs e)
        {
            string fieldShift = "shift_"; // name form for accesing fields in pdf form
            string fieldStart = "start_"; // startShift form for accesing fields in pdf form
            string fieldStop = "stop_";   // stopShift form for accesing fields in pdf form

            int selectedMonth = (int)ListOfMonths.SelectedValue;
            int selectedYear = (int)ListOfYears.SelectedValue;

            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string src = $@"{strPath}pdfForm.pdf";
            string destMerg = $@"{docPath}\pdfOutput\ListaScalona_{new DateTime(2000, selectedMonth, 1).ToString("MMMMMMMMMMM", CultureInfo.CurrentCulture)}.pdf";

            string deletePath = $@"{docPath}\pdfOutput\temp";

            DirectoryInfo di = Directory.CreateDirectory($@"{docPath}\pdfOutput\");

            if (this.selectedWorkers != null)
            {
                PdfDocument mergedPdf = new PdfDocument(new PdfWriter(destMerg));
                PdfMerger merger = new PdfMerger(mergedPdf);

                foreach (var worker in selectedWorkers)
                {
                    var workerName = worker.Name;
                    string monthName;

                    List<Schedule> schedules;
                    List<Shift> shifts;
                    List<Holiday> holidays;

                    if (ListOfMonths.SelectedValue != null)
                    {
                        monthName = new DateTime(2000, selectedMonth, 1).ToString("MMMMMMMMMMM", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        InfoOK info = new InfoOK("Nie wybrano pracowników!");
                        info.ShowDialog();
                        return;
                    }

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<Schedule>(); //read all schedules in selected month/year for specific worker
                        schedules = new List<Schedule>
                            (connection.Table<Schedule>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear) && (c.WorkerId == worker.Id)));

                        connection.CreateTable<Holiday>(); //read all holidays in selected month/year for specific worker
                        holidays = new List<Holiday>
                            (connection.Table<Holiday>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear) && (c.WorkerId == worker.Id)));

                        connection.CreateTable<Shift>();
                        shifts = new List<Shift> //shifts list
                            (connection.Table<Shift>().ToList().OrderBy(c => c.Id).ToList());
                    }
                    Directory.CreateDirectory($@"{ docPath}\pdfOutput\temp");
                    string dest = $@"{docPath}\pdfOutput\temp\{workerName.Replace(" ", "")}.pdf"; // outputPath + generate the workerName pdf

                    PdfDocument pdf = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

                    IDictionary<String, PdfFormField> fields = form.GetFormFields();
                    PdfFormField toSet;
                    fields.TryGetValue("name", out toSet);
                    toSet.SetValue($"{workerName}");

                    fields.TryGetValue("month", out toSet);
                    toSet.SetValue($"{monthName}");

                    for (int day = 1; day <= DateTime.DaysInMonth(selectedYear, selectedMonth); day++)
                    {
                        Schedule daySchedule;

                        if (holidays.Where(c => c.Date.Day == day).Count() >= 1)
                        {

                            fields.TryGetValue(fieldShift + day.ToString(), out toSet);
                            if (holidays.Where(c => c.Date.Day == day).First().Type == "Chorobowe")
                                toSet.SetValue($"N1");
                            else
                                toSet.SetValue($"N2");
                            continue;
                        }

                        if (schedules.Where(c => c.Date.Day == day).Count() == 0)
                        {
                            continue;
                        }

                        daySchedule = schedules.Where(c => c.Date.Day == day).First();

                        if (daySchedule != null)
                        {
                            Shift selectedShift = shifts.Where(c => (c.ShiftName == daySchedule.ShiftName)).ToList().First();

                            fields.TryGetValue(fieldShift + day.ToString(), out toSet);
                            toSet.SetValue($"{daySchedule.ShiftName}");

                            fields.TryGetValue(fieldStart + day.ToString(), out toSet);
                            toSet.SetValue($"{selectedShift.TimeFrom.ToString("H:mm")}");

                            fields.TryGetValue(fieldStop + day.ToString(), out toSet);
                            toSet.SetValue($"{selectedShift.TimeTo.ToString("H:mm")}");
                        }
                    }


                    pdf.Close();

                    PdfDocument pdfForMerge = new PdfDocument(new PdfReader(dest));
                    merger.Merge(pdfForMerge, 1, pdfForMerge.GetNumberOfPages());
                    pdfForMerge.Close();

                }
                mergedPdf.Close();
                Directory.Delete(deletePath, true);
                InfoOK success = new InfoOK("Utworzono pliki PDF");
                success.ShowDialog();
            }
            else
            {
                InfoOK info = new InfoOK("Nie wybrano pracowników!");
                info.ShowDialog();
            }
        }
        /// <summary>
        /// Metoda UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) powoduje wywolanie metody ReadDatabase()
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu DependencyPropertyChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl uc = sender as UserControl;
            if (uc.IsVisible)
            {
                ReadDatabase();
            }
        }

        private void Grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReadDatabase();
        }
    }
}