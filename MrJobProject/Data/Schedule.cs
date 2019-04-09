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
        private int shiftId;

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

        public int ShiftId
        {
            get { return shiftId; }
            set
            {
                if (this.shiftId != value)
                {
                    this.shiftId = value;
                    this.NotifyPropertyChanged("ShiftId");
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
