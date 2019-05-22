using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MrJobProject.Classes
{
    public class Day
    {
        public int DayNumber { get; set; }

        public bool IsHoliday { get; set; }

        public string Reason { get; set; }

        public Day(int day, bool isHoliday)
        {
            DayNumber = day;
            IsHoliday = isHoliday;
            Reason = "";
        }
        public Day(int day, bool isHoliday, string reason)
        {
            DayNumber = day;
            IsHoliday = isHoliday;
            Reason = reason;

        }


    }
}
