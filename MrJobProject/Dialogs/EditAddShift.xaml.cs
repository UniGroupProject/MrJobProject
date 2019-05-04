using MrJobProject.Data;
using System;
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
        }

        public EditAddShift(Shift shift)
        {
            InitializeComponent();

            newShift = shift;
            nameValue.Text = shift.ShiftName;
            timeFromHour.Text = shift.TimeFrom.Hour.ToString();
            timeFromMin.Text = shift.TimeFrom.Minute.ToString();
            timeToHour.Text = shift.TimeTo.Hour.ToString();
            timeToMin.Text = shift.TimeTo.Minute.ToString();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            int fromH = Int32.Parse(timeFromHour.Text);
            int fromM = Int32.Parse(timeFromMin.Text);
            int toH = Int32.Parse(timeToHour.Text);
            int toM = Int32.Parse(timeToMin.Text);

            newShift.ShiftName = nameValue.Text;
            newShift.TimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, fromH, fromM, 0);
            newShift.TimeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, toH, toM, 0);
            this.DialogResult = true;
        }
    }
}