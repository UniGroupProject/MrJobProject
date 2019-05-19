using MrJobProject.Data;
using System.Windows;
using System.Windows.Controls;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Interaction logic for workerUC.xaml
    /// </summary>
    public partial class workerUC : UserControl
    {
        public workerUC()
        {
            InitializeComponent();
        }

        public Worker Worker
        {
            get { return (Worker)GetValue(WorkerProperty); }
            set { SetValue(WorkerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Worker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkerProperty =
            DependencyProperty.Register("Worker", typeof(Worker), typeof(workerUC),
                new PropertyMetadata(new Worker() { Name = "Imie i Nazwisko"}, SetPropertiesOfTextBox));

        private static void SetPropertiesOfTextBox(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            workerUC worker = d as workerUC;

            if (worker != null)
            {
                worker.nameTextBlock.Text = (e.NewValue as Worker).Name;
            }
        }
    }
}