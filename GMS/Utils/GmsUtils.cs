using System;
using System.Text.RegularExpressions;

namespace GMS.Utils
{
    public class GmsUtils
    {
        public static bool IsWeekNumber(string str)
        {
            return new Regex(@"\d{4}w\d{1,2}").IsMatch(str);
        }

        public static string GetMilestoneNumber(DateTime date)
        {
            var weekNumber = DateTimeUtils.GetIso8601WeekOfYear(date).ToString();
            if (weekNumber.Length == 1)
            {
                weekNumber = $"0{weekNumber}";
            }
            return $"{date:yyyy}w{weekNumber}";
        }
    }
}