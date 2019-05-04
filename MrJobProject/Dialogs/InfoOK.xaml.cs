using System.Windows;

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