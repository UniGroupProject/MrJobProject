using MrJobProject.Data;
using NUnit.Framework;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class WorkerTests
    {
        [Test]
        public void Is_id_field_not_null()
        {
            var worker = new Worker();
            Assert.IsNotNull(worker.Id);
        }

        [TestCase("Jan Kowalski")]
        [TestCase("Andrzej Duda")]
        [TestCase("Wielka Pierdola")]
        public void Is_workerName_filled_with_given_name(string name)
        {
            var worker = new Worker() { Name = name };
            Assert.AreEqual(name, worker.Name);

        }

        [TestCase(true)]
        [TestCase(false)]
        public void Is_workerStatus_filled_with_given_boolean(bool arg)
        {
            var worker = new Worker() {Status = arg};
            Assert.AreEqual(arg, worker.Status);

        }

    }
}