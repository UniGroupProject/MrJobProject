using MrJobProject.Data;
using MrJobProject.UserControllers;
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

namespace MrJobProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HolidayDateUC holiday;
        public MainWindow()
        {
            InitializeComponent();
           // GridCursor.Width = (Application.Current.MainWindow.Width - 20) / 4; // <------------------- width of thickness
        }

        public void ChangeToHolidayDateUC(Worker worker)
        {
            HolidayGrid.Visibility = Visibility.Hidden;
            holiday = new HolidayDateUC(worker);
            MainView.Children.Add(holiday);
            
        }

        public void BackToHolidayUC(Worker worker)
        {

            MainView.Children.Remove(holiday);
            HolidayGrid.Visibility = Visibility.Visible;

        }

        private void MenuClick(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursor.SetValue(Grid.ColumnProperty, index);
            //GridCursor.Margin = new Thickness(10+ ((Application.Current.MainWindow.Width - 40)/4 * index), 0, 0, 0); // <------ align of thickness
            ScheduleGrid.Visibility = Visibility.Hidden;
            WorkersGrid.Visibility = Visibility.Hidden;
            HolidayGrid.Visibility = Visibility.Hidden;
            ListsGrid.Visibility = Visibility.Hidden;
            switch (index)
            {
                case 0:
                    ScheduleGrid.Visibility = Visibility.Visible;
                    break;
                case 1:
                    WorkersGrid.Visibility = Visibility.Visible;
                    break;
                case 2:
                    HolidayGrid.Visibility = Visibility.Visible;
                    break;
                case 3:
                    ListsGrid.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
