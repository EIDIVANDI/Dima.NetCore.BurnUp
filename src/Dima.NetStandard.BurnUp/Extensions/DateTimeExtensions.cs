using System;

namespace Dima.NetStandard.BurnUp.Extensions
{
    public static class DateTimeExtensions
    {
        public static double GetTodayInSeconds(this DateTime date)
        {
            return DateTime.Now.GetDiffInSeconds(new DateTime(1970, 1, 9, 0, 0, 00));
        }

        public static double GetDiffInSeconds(this DateTime date, DateTime date2)
        {
            TimeSpan nowDiff = date - date2;
            return nowDiff.TotalSeconds;
        }
    }
}
