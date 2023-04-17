using System;
using System.Globalization;

namespace GMS.Utils
{
    public class DateTimeUtils
    {
        
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime ParseToDate(string str)
        {
            return DateTime.ParseExact(str, "yyyy-MM-dd", null);
        }
        
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek? day = null)
        {
            day ??= start.DayOfWeek;
            
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            var daysToAdd = ((int) day - (int) start.DayOfWeek + 7) % 7;
            if (start.DayOfWeek == day)
            {
                daysToAdd = 7;
            }
            return start.AddDays(daysToAdd);
        }
    }
}