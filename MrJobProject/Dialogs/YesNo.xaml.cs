using System.Windows;

namespace MrJobProject.Dialogs
{
    /// <summary>
    /// Interaction logic for YesNo.xaml
    /// </summary>
    public partial class YesNo : Window
    {
        public YesNo(string text)
        {
            InitializeComponent();
            question.Text = text;
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}