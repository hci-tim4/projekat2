using System;

namespace railway.drivingLineReport
{
    public static class DateTimeToMonthAndYearString
    {
        public static string Convert(DateTime date)
        {
            return date.ToString("MM.yyyy");
        } 
    }
}