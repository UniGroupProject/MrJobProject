using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrJobProject.Classes;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    [TestFixture]
    class DayTests
    {
        [Test]

        [TestCase(25, true, "powod urlopu to test jednostkowy")]
        public void Is_holiday_true_check(int dayNumber,bool isHoliday, string reason)
        {
            var day = new Day(dayNumber,isHoliday,reason);
            bool IsHoliday = day.IsHoliday;
            Assert.True(IsHoliday);
        }
        [Test]
        public void Day_is_in_range_check([Random(1, 31, 12)]int dayNumber)
        {
            var day = new Day(dayNumber, false);
            Assert.AreEqual(dayNumber,day.DayNumber);
            

        }
    }
}
