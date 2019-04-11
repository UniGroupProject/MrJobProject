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
