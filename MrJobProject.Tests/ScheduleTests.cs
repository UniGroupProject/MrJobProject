using MrJobProject.Data;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class ScheduleTests
    {
        [Test]
        public void Is_id_field_not_null()
        {
            var schedule = new Schedule();
            Assert.IsNotNull(schedule.Id);
        }


        [Test]
        public void Is_schedule_filled_with_given_workerID([Random(1, 10000, 30)]int workerID)
        {
            var schedule = new Schedule() { WorkerId = workerID };
            Assert.AreEqual(workerID, schedule.WorkerId);

        }
        [TestCase("A")]
        [TestCase("B")]
        [TestCase("C")]
        public void Is_schedule_filled_with_given_type(string type)
        {
            var schedule = new Schedule() { ShiftName = type };
            Assert.AreEqual(type, schedule.ShiftName);

        }
    }
}