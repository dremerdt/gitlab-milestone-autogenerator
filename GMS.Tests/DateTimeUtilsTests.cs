using System;
using GMS.Utils;
using Xunit;

namespace GMS.Tests
{
    public class DateTimeUtilsTests
    {
        [Fact]
        public void GetIso8601WeekOfYear_ShouldGetProperlyWeekNumber_Gets()
        {
            var date = new DateTime(2021, 08, 22);

            var weekNumber33 = DateTimeUtils.GetIso8601WeekOfYear(date);
            var weekNumber34 = DateTimeUtils.GetIso8601WeekOfYear(date.AddDays(1));
            
            Assert.Equal(33, weekNumber33);
            Assert.Equal(34, weekNumber34);
        }

        [Fact]
        public void ParseToDate_ShouldParse_Parsed()
        {
            var dateStr = "2021-08-22";

            var date = DateTimeUtils.ParseToDate(dateStr);
            
            Assert.Equal(2021, date.Year);
            Assert.Equal(08, date.Month);
            Assert.Equal(22, date.Day);
        }

        [Fact]
        public void GetNextWeekday_ShouldIncrementWeekNumber_NextWeekChecked()
        {
            var date = new DateTime(2021, 08, 22);
            var weekNumber33 = DateTimeUtils.GetIso8601WeekOfYear(date);

            var nextWeekDate = DateTimeUtils.GetNextWeekday(date);
            var weekNumber34 = DateTimeUtils.GetIso8601WeekOfYear(nextWeekDate);
            
            Assert.Equal(33, weekNumber33);
            Assert.Equal(34, weekNumber34);
        }
    }
}