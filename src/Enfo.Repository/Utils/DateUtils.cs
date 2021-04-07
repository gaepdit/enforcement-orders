using System;

namespace Enfo.Repository.Utils
{
    public static class DateUtils
    {
        /// <summary>
        /// Finds the date of the next desired day of the week including 
        /// or immediately following a specified date.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="day">The desired day of week.</param>
        /// <returns>A DateTime that is the desired day of the week, including 
        /// or immediately following the start date.</returns>
        /// <remarks>http://stackoverflow.com/a/6346190/212978</remarks>
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day) =>
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            start.AddDays(((int) day - (int) start.DayOfWeek + 7) % 7);

        public static DateTime MostRecentMonday() =>
            GetNextWeekday(DateTime.Today.AddDays(-6), DayOfWeek.Monday);

        public static DateTime NextMonday() =>
            GetNextWeekday(DateTime.Today.AddDays(1), DayOfWeek.Monday);
    }
}