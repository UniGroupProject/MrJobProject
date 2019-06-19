using SQLite;
using System;
using System.ComponentModel;

namespace MrJobProject.Data
{
    /// <summary>
    ///  Klasa Schedule, wykorzystana przez baze danych SQLite oraz aplikacje do przechowywania danych zwiazanych z grafikiem pracy
    /// </summary>
    /// <remarks>
    /// Zawiera wlasciwosci oraz zdarzenie
    /// </remarks>
    public class Schedule : INotifyPropertyChanged
    {
        private int id;
        private DateTime date;
        private int workerId;
        private string shiftName;
        /// <value>Zwraca lub ustawia Id grafika, jest primary key-em</value>
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <value>Zwraca lub ustawia date dotyczaca danego dnia z grafika</value>
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
        /// <value>Zwraca lub ustawia id pracownika, ktorego dotyczy grafik</value>
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
        /// <value>Zwraca lub ustawia nazwe zmiany, ktora dotyczy grafika</value>
        public string ShiftName
        {
            get { return shiftName; }
            set
            {
                if (this.shiftName != value)
                {
                    this.shiftName = value;
                    this.NotifyPropertyChanged("ShiftName");
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