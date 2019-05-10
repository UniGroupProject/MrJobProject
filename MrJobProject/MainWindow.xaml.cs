using MrJobProject.Data;
using MrJobProject.UserControllers;
using System.Windows;
using System.Windows.Controls;

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