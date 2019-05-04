using MrJobProject.Data;
using MrJobProject.UserControllers;
using System.Windows;

namespace MrJobProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Holiday.Children.Add(new HolidayUC());
        }

        public void ChangeToHolidayDateUC(Worker worker)
        {
            Holiday.Children.Clear();
            Holiday.Children.Add(new HolidayDateUC(worker));
        }

        public void BackToHolidayUC(Worker worker)
        {
            Holiday.Children.Clear();
            Holiday.Children.Add(new HolidayUC());
        }
    }
}