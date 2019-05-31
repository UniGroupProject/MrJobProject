using MrJobProject.Data;
using System.Windows;
using System.Windows.Controls;

namespace MrJobProject.UserControllers
{
    /// <summary>
    /// Logika kontrolki workerUC
    /// </summary>
    /// <remarks>
    /// Zawiera konstruktor, wlasciwosc oraz rejestruje dependency property
    /// </remarks>
    public partial class workerUC : UserControl
    {
        public workerUC()
        {
            InitializeComponent();
        }
        /// <value>Zwraca lub ustawia wlasciwosc typu Worker</value>
        public Worker Worker
        {
            get { return (Worker)GetValue(WorkerProperty); }
            set { SetValue(WorkerProperty, value); }
        }

        /// <value>Rejestruje dependency property</value>
        public static readonly DependencyProperty WorkerProperty =
            DependencyProperty.Register("Worker", typeof(Worker), typeof(workerUC),
                new PropertyMetadata(new Worker() { Name = "Imięs i Nazwisko"}, SetPropertiesOfTextBox));
        /// <value>Ustawia wyswietlana nazwe pracownika w kontrolce</value>
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