using System.Globalization;
using NUnit.Framework;
using  MrJobProject.Converters;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class BoolToActiveConverterTests
    {
        [TestCase(true)]
        public void Is_converter_returning_active_to_true_argument(bool value)
        {
            var conv = new BoolToActiveConverter();
            Assert.AreEqual("Active",conv.Convert(value,typeof(string),null,CultureInfo.CurrentCulture));
        }

    }
}