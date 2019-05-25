using MrJobProject.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Interaction logic for EditAddShift.xaml
    /// </summary>
    public partial class EditAddShift : Window
    {
        public Shift newShift;

        public EditAddShift()
        {
            InitializeComponent();
            newShift = new Shift();

            timeFromHour.Text = "00";
            timeFromMin.Text = "00";
            timeToHour.Text = "00";
            timeToMin.Text = "00";

            nameValue.Focus();
        }

        public EditAddShift(Shift shift)
        {
            InitializeComponent();

            newShift = shift;
            nameValue.Text = shift.ShiftName;
            timeFromHour.Text = shift.TimeFrom.ToString("HH");
            timeFromMin.Text = shift.TimeFrom.ToString("mm");
            timeToHour.Text = shift.TimeTo.ToString("HH");
            timeToMin.Text = shift.TimeTo.ToString("mm");

            nameValue.Focus();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            bool noExept = true;
            
            try
            {
                int fromH = Int32.Parse(timeFromHour.Text);
                int fromM = Int32.Parse(timeFromMin.Text);
                int toH = Int32.Parse(timeToHour.Text);
                int toM = Int32.Parse(timeToMin.Text);

                newShift.ShiftName = nameValue.Text;

                newShift.TimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fromH, fromM, 0);
                newShift.TimeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, toH, toM, 0);
            }
            
            catch(Exception)
            {
                InfoOK error = new InfoOK("Nieprawidłowe dane. Spróbuj ponownie");
                error.ShowDialog();
                noExept = false;
            }

            List<Shift> shifts;
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Shift>();
                shifts = new List<Shift> //shifts list
                    (connection.Table<Shift>().Where(c => c.ShiftName == newShift.ShiftName).ToList());
            }
            if(shifts.Count > 0)
            {
                InfoOK error = new InfoOK("Zamiana o danej nazwie już istnieje");
                error.ShowDialog();
                noExept = false;
            }

            if (noExept && newShift.ShiftName != "")
            this.DialogResult = true;
        }

        private void TimeFromHour_GotFocus(object sender, RoutedEventArgs e)
        {
            timeFromHour.SelectAll();
        }

        private void TimeFromMin_GotFocus(object sender, RoutedEventArgs e)
        {
            timeFromMin.SelectAll();
        }

        private void TimeToHour_GotFocus(object sender, RoutedEventArgs e)
        {
            timeToHour.SelectAll();
        }

        private void TimeToMin_GotFocus(object sender, RoutedEventArgs e)
        {
            timeToMin.SelectAll();
        }
    }
}