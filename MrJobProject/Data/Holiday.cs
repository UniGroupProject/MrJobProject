using SQLite;
using System;
using System.ComponentModel;

namespace MrJobProject.Data
{
    public  class Holiday : INotifyPropertyChanged
    {
        private int id;
        private int workerId;
        private DateTime date;
        private string type;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}