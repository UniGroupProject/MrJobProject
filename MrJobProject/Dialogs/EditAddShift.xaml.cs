using MrJobProject.Data;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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

            timeFrom.Text = "00";
            timeTo.Text = "00";

        }
        public EditAddShift(Shift shift)
        {
            InitializeComponent();

            newShift = shift;
            nameValue.Text = shift.ShiftName;
            timeFrom.Text = shift.TimeFrom.Hour.ToString();
            timeTo.Text = shift.TimeTo.Hour.ToString();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            int from = Int32.Parse(timeFrom.Text); 
            int to = Int32.Parse(timeTo.Text); 
            
            newShift.ShiftName = nameValue.Text;
            newShift.TimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, from, 0, 0);
            newShift.TimeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, to, 0, 0);
            this.DialogResult = true;
        }
    }
}
