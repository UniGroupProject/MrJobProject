using System;
using System.Globalization;
using NUnit.Framework;
using MrJobProject.Converters;
namespace MrJobProject.Tests
{
    [TestFixture]
    public class TimeToStringConverterTests
    {
        [TestCase(10,15,"10:15")]
        [TestCase(20, 32, "20:32")]
        [TestCase(11, 11, "11:11")]
        [TestCase(23, 59, "23:59")]
        public void Is_converter_returning_correct_string_from_given_datetime_type(int hour, int min, string hourAndMinString)
        {
            var time=new DateTime(2000,1,2,hour,min,5);
            var conv = new TimeToStringConverter();
            Assert.AreEqual(hourAndMinString,conv.Convert(time,typeof(string),null,CultureInfo.CurrentCulture));
        }

    }
}