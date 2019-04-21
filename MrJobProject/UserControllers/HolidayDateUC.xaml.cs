using MrJobProject.Data;
using MrJobProject.Dialogs;
using SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for HolidayDateUC.xaml
    /// </summary>
    public partial class HolidayDateUC : UserControl
    {
        Worker worker;
        public HolidayDateUC(Worker worker)
        {
            this.worker = worker;

            InitializeComponent();
            WorkerName.Text = worker.Name;
            ListOfHolidayTypes.ItemsSource = GetListOfHolidayTypes(); // to do: maybe insert to database

        }

        private List<string> GetListOfHolidayTypes()
        {
            List<string> HolidayTypes = new List<string>();//language item
            HolidayTypes.Add("choroba pracownika (płaci pracodawca)");
            HolidayTypes.Add("choroba pracownika (płaci ZUS)");
            HolidayTypes.Add("choroba niepłatne");
            HolidayTypes.Add("opieka");
            HolidayTypes.Add("wypoczynkowy");
            HolidayTypes.Add("na żadanie");
            HolidayTypes.Add("bezpłatny");
            HolidayTypes.Add("okolicznościowy");
            HolidayTypes.Add("macierzyński");
            HolidayTypes.Add("wychowawczy");
            HolidayTypes.Add("nieobecność usprawiedliwiona bezpłatna");
            HolidayTypes.Add("inne obecności nieusprawiediwione");
            return HolidayTypes;
        }

        private void Back(object sender, RoutedEventArgs e) //back button
        {
            var mainWindow = Application.Current.Windows[0] as MainWindow;
            mainWindow.BackToHolidayUC(worker);
        }

        private void SaveChangesBtn(object sender, RoutedEventArgs e)
        {
            if (ListOfHolidayTypes.SelectedItem == null || TheCalendar.SelectedDates.Count == 0)
            {
                if (ListOfHolidayTypes.SelectedItem == null)
                {
                    InfoOK info = new InfoOK("Type not selected");
                    if (info.ShowDialog() == true) return;
                }
                else if (TheCalendar.SelectedDates.Count == 0)
                {
                    InfoOK info = new InfoOK("Dates not selected");
                    if (info.ShowDialog() == true) return;
                }
            }
            string selectedType = ListOfHolidayTypes.SelectedItem.ToString();

            using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
            {
                foreach (var item in TheCalendar.SelectedDates)
                {
                    connection.CreateTable<Holiday>();

                    var day = connection.Table<Holiday>().Where(c => c.Date == item).ToList(); //check if there is any item saved in this date
                    if (day.Count > 1) // if there is more item with one date, error
                    {
                        MessageBox.Show("Problem z baza, wiecej niz 1 urlop w 1 dzien");
                    }
                    else if (day.Count == 1) // date was used before, so just change values
                    {
                        var oneDay = day.First();
                        oneDay.Type = selectedType;
                        connection.InsertOrReplace(oneDay);
                    }
                    else // if date not used before, add it
                    {
                        Holiday holiday = new Holiday()
                        { WorkerId = worker.Id, Date = item, Type = selectedType };

                        connection.Insert(holiday);
                    }
                }
            }
            TheCalendar.SelectedDates.Clear();
        }

        private void DatacontextItem_Delete(object sender, RoutedEventArgs e) // right click -> delete dates
        {
            if (TheCalendar.SelectedDates.Count == 0)
            {
                InfoOK info = new InfoOK("Dates not selected");
                if (info.ShowDialog() == true) return;
            }
            string question = "Are you sure you want to delete?"; //language item
            YesNo Result = new YesNo(question);

            if (Result.ShowDialog() == true)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                {
                    foreach (var item in TheCalendar.SelectedDates)
                    {
                        connection.CreateTable<Holiday>();

                        var dates = connection.Table<Holiday>().Where(c => c.Date == item).ToList(); //check if there is any item saved in this date
                        foreach (var date in dates)
                        {
                            connection.Delete(date);//remove items
                        }
                    }
                }
                TheCalendar.SelectedDates.Clear();
            }
        }
    }
}
