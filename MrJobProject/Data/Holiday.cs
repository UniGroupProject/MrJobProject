using SQLite;
using System;
using System.ComponentModel;

namespace MrJobProject.Data
{
    /// <summary>
    ///  Klasa Holiday, wykorzystana przez baze danych SQLite oraz aplikacje do przechowywania danych zwiazanych z urlopami
    /// </summary>
    /// <remarks>
    /// Zawiera wlasciwosci oraz zdarzenie
    /// </remarks>
    public class Holiday : INotifyPropertyChanged
    {
        private int id;
        private int workerId;
        private DateTime date;
        private string type;
        /// <value>Zwraca lub ustawia id urlopu, jest to primary key</value>
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <value>Zwraca lub ustawia id pracownika, ktorego dotyczy urlop</value>
        public int WorkerId
        {
            get { return workerId; }
            set
            {
                if (this.workerId != value)
                {
                    this.workerId = value;
                    this.NotifyPropertyChanged("WorkerId");
                }
            }
        }
        /// <value>Zwraca lub ustawia date urlopu</value>
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (this.date != value)
                {
                    this.date = value;
                    this.NotifyPropertyChanged("Date");
                }
            }
        }
        /// <value>Zwraca lub ustawia powod urlopu(string)</value>
        public string Type
        {
            get { return type; }
            set
            {
                if (this.type != value)
                {
                    this.type = value;
                    this.NotifyPropertyChanged("Type");
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///  Metoda NotifyPropertyChanged, informuje o zmianie w danej wlasciwosci
        /// </summary>
        /// <param name="propName">Argument typu string, ktory przekazuje nazwe wlasciwosci</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}