using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrJobProject.Data
{
    class Schedule : INotifyPropertyChanged
    {
        private int id;
        private DateTime date;
        private int workerId;
        private string shiftName;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { id = value; }
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

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
