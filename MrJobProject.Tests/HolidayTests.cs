using MrJobProject.Data;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class HolidayTests
    {
        [Test]
        public void Is_id_field_not_null()
        {
            var holiday = new Holiday();
            Assert.IsNotNull(holiday.Id);
        }

        
        [Test]
        public void Is_holiday_filled_with_given_workerID([Random(1, 10000, 30)]int workerID)
        {
            var holiday = new Holiday() {WorkerId = workerID};
            Assert.AreEqual(workerID, holiday.WorkerId);

        }
        [TestCase("L4")]
        [TestCase("urodzinu")]
        [TestCase("bez zus")]
        public void Is_holiday_filled_with_given_type(string type)
        {
            var holiday = new Holiday() { Type = type};
            Assert.AreEqual(type, holiday.Type);

        }

    }
}