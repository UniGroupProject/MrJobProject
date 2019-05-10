using SQLite;
using System;
using System.ComponentModel;

namespace MrJobProject.Data
{
    public class Shift : INotifyPropertyChanged
    {
        private int id;
        private string shiftName;
        private DateTime timeFrom;
        private DateTime timeTo;
        private int minNumberOfWorkers;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}