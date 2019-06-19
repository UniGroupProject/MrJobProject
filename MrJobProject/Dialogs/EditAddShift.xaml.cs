using MrJobProject.Data;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Logika okna dialogowego EditAddShift
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor oraz pole
    /// </remarks>
    public partial class EditAddShift : Window
    {
        /// <summary>
        /// zmienna typu Shift o nazwie newShift informacje o nowej zmianie
        /// </summary>
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
        /// <summary>
        /// Metoda SaveBtn_Click(object sender, TextChangedEventArgs e) podczas wywolania edytuje wybrana zmiane i zapisuje w bazie danych 
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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

        /// <summary>
        /// Metoda TimeFromHour_GotFocus(object sender, TextChangedEventArgs e) podczas wywolania zaznacza tekst w wybranych polu teksowym
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void TimeFromHour_GotFocus(object sender, RoutedEventArgs e)
        {
            timeFromHour.SelectAll();
        }

        /// <summary>
        /// Metoda TimeFromMin_GotFocus(object sender, TextChangedEventArgs e) podczas wywolania zaznacza tekst w wybranych polu teksowym
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void TimeFromMin_GotFocus(object sender, RoutedEventArgs e)
        {
            timeFromMin.SelectAll();
        }

        /// <summary>
        /// Metoda TimeToHour_GotFocus(object sender, TextChangedEventArgs e) podczas wywolania zaznacza tekst w wybranych polu teksowym
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void TimeToHour_GotFocus(object sender, RoutedEventArgs e)
        {
            timeToHour.SelectAll();
        }

        /// <summary>
        /// Metoda TimeToMin_GotFocus(object sender, TextChangedEventArgs e) podczas wywolania zaznacza tekst w wybranych polu teksowym
        /// </summary>
        /// <param name="sender">Argument typu object, ktory przekazuje obiekt</param>
        /// <param name="e">Argument typu RoutedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
        private void TimeToMin_GotFocus(object sender, RoutedEventArgs e)
        {
            timeToMin.SelectAll();
        }
    }
}