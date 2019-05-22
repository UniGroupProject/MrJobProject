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

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for ListsUC.xaml
    /// </summary>
    public partial class ListsUC : UserControl
    {
        private ObservableCollection<Worker> workers;
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox searchTextBox = sender as TextBox;
            var filteredList = workers.Where(c => c.Name.ToLower().Contains(searchTextBox.Text)).ToList();

            workersListView.ItemsSource = filteredList;
        }

        private void WorkersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedWorkers = workersListView.SelectedItems.Cast<Worker>().ToList();

        }

        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in workers)
            {
                workersListView.SelectedItems.Clear();
            }
            Keyboard.Focus(workersListView);
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            workersListView.SelectAll();
        }

        private void PdfButton_OnClick(object sender, RoutedEventArgs e)
        {
            string fieldShift = "shift_"; // name form for accesing fields in pdf form
            string fieldStart = "start_"; // startShift form for accesing fields in pdf form
            string fieldStop = "stop_";   // stopShift form for accesing fields in pdf form

            string strPath = System.AppDomain.CurrentDomain.BaseDirectory;


            if (this.selectedWorkers != null)
            {
                foreach (var worker in selectedWorkers)
                {
                    int selectedMonth = (int)ListOfMonths.SelectedValue;
                    int selectedYear = (int)ListOfYears.SelectedValue;
                    var workerName = worker.Name;
                    string monthName;

                    List<Schedule> schedules;
                    List<Shift> shifts;

                    if (ListOfMonths.SelectedValue != null)
                    {
                        monthName = new DateTime(2000, selectedMonth, 1).ToString("MMMMMMMMMMM", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        MessageBox.Show("ERROR NO MONTH SELECTED");
                        return;
                    }

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {


                        connection.CreateTable<Schedule>(); //read all schedules in selected month/year for specific worker
                        schedules = new List<Schedule>
                            (connection.Table<Schedule>().ToList().Where(c => (c.Date.Month == selectedMonth) && (c.Date.Year == selectedYear) && (c.WorkerId == worker.Id)));

                        connection.CreateTable<Shift>();
                        shifts = new List<Shift> //shifts list
                            (connection.Table<Shift>().ToList().OrderBy(c => c.Id).ToList());

                    }

                    string src = $@"{strPath}pdfForm.pdf";
                    string dest = $@"{strPath}pdfOutput\{workerName.Replace(" ", "")}.pdf"; // outputPath + generate the workerName pdf

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
                }
            }
            else
            {
                MessageBox.Show("No workers selected!");
            }



        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UserControl uc = sender as UserControl;
            if (uc.IsVisible)
            {
                ReadDatabase();
            }
        }
    }
}