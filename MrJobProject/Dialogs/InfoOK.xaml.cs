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
    /// Interaction logic for InfoOK.xaml
    /// </summary>
    public partial class InfoOK : Window
    {
        public InfoOK(string text)
        {
            InitializeComponent();
            question.Text = text;
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }
    }
}
