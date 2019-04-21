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
    /// Interaction logic for ScheduleUC.xaml
    /// </summary>
    public partial class ScheduleUC : UserControl
    {
        ObservableCollection<Shift> shifts;
        ObservableCollection<Worker> workers;


        public ScheduleUC()
        {
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
        }

        private void UpdateList()
        {
            ReadDatabase();
            ShiftList.ItemsSource = shifts;
            WorkersList.ItemsSource = workers;

        }

        private void DeleteShift(object sender, RoutedEventArgs e)
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

        private void AddShiftDialog(object sender, RoutedEventArgs e)
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

        private void EditShiftDialog(object sender, RoutedEventArgs e)
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void BackDateBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedYear = (int)ListOfYears.SelectedValue;
            int selectedMonth = (int)ListOfMonths.SelectedValue;
            if (selectedMonth == 1)
            {
                int size = ListOfYears.Items.Count;
                if (selectedYear == (int)ListOfYears.Items.GetItemAt(size-1)) return;

                ListOfMonths.SelectedValue = 12;
                ListOfYears.SelectedValue = selectedYear - 1;
            }
            else
            {
                ListOfMonths.SelectedValue = selectedMonth - 1;
            }
        }

        private void ForwardDateBtn_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
