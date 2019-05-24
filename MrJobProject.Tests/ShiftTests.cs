using MrJobProject.Data;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class ShiftTests
    {

        [Test]
        public void Is_id_field_not_null()
        {
            var shift = new Shift();
            Assert.IsNotNull(shift.Id);
        }


        [Test]
        public void Is_shiftMinimumNumberOfWorkers_filled_with_given_integer([Random(1, 10000, 30)]int minNumber)
        {
            var shift = new Shift() { MinNumberOfWorkers = minNumber };
            Assert.AreEqual(minNumber, shift.MinNumberOfWorkers);

        }
        [TestCase("A")]
        [TestCase("B")]
        [TestCase("C")]
        public void Is_shiftName_filled_with_given_name(string name)
        {
            var shift = new Shift() { ShiftName = name};
            Assert.AreEqual(name, shift.ShiftName);

        }

    }
}