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
using MrJobProject.Data;
using MrJobProject.Classes;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for DayUC.xaml
    /// </summary>
    public partial class DayUC : UserControl
    {

        public DayUC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day", typeof(Day), typeof(DayUC), new PropertyMetadata(new Day(1,false),SetDayTextAndBackground));

        private static void SetDayTextAndBackground(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DayUC day = d as DayUC;
            if (day != null)
            {
                day.DayNumberTextBlock.Text = (e.NewValue as Day).DayNumber.ToString();
                if ((e.NewValue as Day).IsHoliday == true)
                {
                    day.Background = Brushes.Coral;
                    day.ReasonTextBlock.Text = (e.NewValue as Day).Reason;
                }
                else
                {
                    day.ReasonTextBlock.Text = "";
                }
            }
        }

        public Day Day
        {
            get { return (Day) GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

    }
}