using SQLite;
using System.ComponentModel;

namespace MrJobProject.Data
{
    /// <summary>
    ///  Klasa Worker, wykorzystana przez baze danych SQLite oraz aplikacje do przechowywania danych zwiazanych z pracownikami
    /// </summary>
    /// <remarks>
    /// Zawiera wlasciwosci oraz zdarzenie
    /// </remarks>
    public class Worker : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private bool status;
        /// <value>Zwraca lub ustawia Id pracownika, jest primary key-em</value>
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <value>Zwraca lub ustawia nazwe pracownika/value>
        public string Name
        {
            get { return name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }
        /// <value>Zwraca lub ustawia status pracownika/value>
        public bool Status
        {
            get { return status; }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///  Metoda NotifyPropertyChanged, informuje o zmianie w danej wlasciwosci
        /// </summary>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}