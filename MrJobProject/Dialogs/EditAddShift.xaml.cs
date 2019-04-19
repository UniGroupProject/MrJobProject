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
        }
        public EditAddShift(Shift shift)
        {
            InitializeComponent();

            newShift = shift;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
