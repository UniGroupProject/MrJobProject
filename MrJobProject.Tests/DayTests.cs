using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrJobProject.Classes;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    class DayTests
    {
        [Test]

        [TestCase(25, true, "powod urlopu to test jednostkowy")]
        public void IsHoliday_CheckIfTrue(int dayNumber,bool isHoliday, string reason)
        {
            var day = new Day(dayNumber,isHoliday,reason);
            bool IsHoliday = day.IsHoliday;
            Assert.True(IsHoliday);
        }
        [Test]
        public void Day_IsInMonthRange([Random(1, 31, 12)]int dayNumber)
        {
            var day = new Day(dayNumber, false);
            Assert.AreEqual(dayNumber,day.DayNumber);

        }
    }
}
