using System.Globalization;
using NUnit.Framework;
using MrJobProject.Converters;

namespace MrJobProject.Tests
{
    [TestFixture]
    public class NumberToMonthConverterTests
    {
        [TestCase(1,"January")]
        [TestCase(2, "February")]
        [TestCase(5, "May")]
        public void Is_converter_returning_correct_monthName_to_given_number(int montNumber,string monthName)
        {
            var conv= new NumberToMonthConverter();
            Assert.AreEqual(monthName,conv.Convert(montNumber,typeof(string),null,CultureInfo.CurrentCulture));
        }
    }
}