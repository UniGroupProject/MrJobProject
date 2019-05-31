using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MrJobProject.Classes
{
    /// <summary>
    ///  Klasa Day, wykorzystana w kontrolce DayUC do przedstawienia poszczegolnych dni i tego, czy sa urlopami
    /// </summary>
    /// <remarks>
    /// Klasa Day zawiera jedynie wlasciwosci i konstruktory
    /// </remarks>
    public class Day
    {
        /// <value>Zwraca lub ustawia numer dnia</value>
        public int DayNumber { get; set; }
        /// <value>Zwraca lub ustawia stan, czy dany dzien jest dniem urlopu</value>
        public bool IsHoliday { get; set; }
        /// <value>Zwraca lub ustawia powod urlopu</value>
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
