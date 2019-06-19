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
    /// Logika kontrolki DayUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, wlasciwosc oraz rejestruje dependency property
    /// </remarks>
    public partial class DayUC : UserControl
    {

        public DayUC()
        {
            InitializeComponent();
        }
        /// <value>Rejestruje dependency property</value>
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day", typeof(Day), typeof(DayUC), new PropertyMetadata(new Day(1,false),SetDayTextAndBackground));

        /// <value>Ustawia text kontrolki oraz kolor tla, zaleznie od tego, czy dany dzien jest urlopem</value>
        /// <param name="d">Argument typu DependencyObject, ktory przekazuje obiekt, ktory uczestniczy w systemie wlasciwosci zaleznosci.</param>
        /// <param name="e">Argument typu DependencyPropertyChangedEventArgs, ktory przekazuje wszystkie informacje o zdarzeniu </param>
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

        /// <value>Zwraca lub ustawia wlasciwosc typu Day</value>
        public Day Day
        {
            get { return (Day) GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

    }
}