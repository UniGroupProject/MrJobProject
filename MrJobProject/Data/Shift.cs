using SQLite;
using System;
using System.ComponentModel;

namespace MrJobProject.Data
{
    /// <summary>
    ///  Klasa Shift, wykorzystana przez baze danych SQLite oraz aplikacje do przechowywania danych zwiazanych z zmianami
    /// </summary>
    /// /// <remarks>
    /// Zawiera wlasciwosci oraz zdarzenie
    /// </remarks>
    public class Shift : INotifyPropertyChanged
    {
        private int id;
        private string shiftName;
        private DateTime timeFrom;
        private DateTime timeTo;
        private int minNumberOfWorkers;
        /// <value>Zwraca lub ustawia Id zmiany, jest primary key-em</value>
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <value>Zwraca lub ustawia nazwe zmiany/value>
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
        /// <value>Zwraca lub ustawia godzine startowa zmiany</value>
        public DateTime TimeFrom
        {
            get { return timeFrom; }
            set
            {
                if (this.timeFrom != value)
                {
                    this.timeFrom = value;
                    this.NotifyPropertyChanged("TimeFrom");
                }
            }
        }
        /// <value>Zwraca lub ustawia godzine koncowa zmiany</value>
        public DateTime TimeTo
        {
            get { return timeTo; }
            set
            {
                if (this.timeTo != value)
                {
                    this.timeTo = value;
                    this.NotifyPropertyChanged("TimeTo");
                }
            }
        }
        /// <value>Zwraca lub ustawia minimalna ilosc pracownikow na zmiane</value>
        public int MinNumberOfWorkers
        {
            get { return minNumberOfWorkers; }
            set
            {
                if (this.minNumberOfWorkers != value)
                {
                    this.minNumberOfWorkers = value;
                    this.NotifyPropertyChanged("MinNumberOfWorkers");
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