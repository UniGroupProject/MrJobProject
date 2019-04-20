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

        public ScheduleUC()
        {
            InitializeComponent();


            shifts = new ObservableCollection<Shift>();

            UpdateList();
        }

        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                connection.CreateTable<Shift>();
                shifts = new ObservableCollection<Shift>
                    (connection.Table<Shift>().ToList().OrderBy(c => c.Id).ToList());
            }
        }

        private void UpdateList()
        {
            ReadDatabase();
            ShiftList.ItemsSource = shifts;
        }

        private void DeleteShift(object sender, RoutedEventArgs e)
        {
            Shift shift = ShiftList.SelectedItem as Shift;
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

        private void AddShiftDialog(object sender, RoutedEventArgs e)
        {
            EditAddShift addShift = new EditAddShift();
            if (addShift.ShowDialog() == true)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    connection.CreateTable<Shift>();
                    connection.InsertOrReplace(addShift.newShift);
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
    }
}
