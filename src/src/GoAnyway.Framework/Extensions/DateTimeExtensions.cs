using GoAnyway.Framework.Time;

namespace GoAnyway.Framework.Extensions;

public static class DateTimeExtensions
{
    public static DateTime DaysBefore(
        this DateTime from,
        int days)
    {
        return from.Subtract(TimeSpan.FromDays(days));
    }

    public static DateTime TimeBefore(
        this DateTime from,
        TimeSpan time)
    {
        return from.Subtract(time);
    }

    public static DateTimeInterval Month(this DateTime now)
    {
        var from = new DateTime(
            year: now.Year,
            month: now.Month,
            day: 1
        );

        return new(
            From: from,
            To: from.AddMonths(1)
        );
    }

    public static bool IsTimeoutPassed(
        this DateTime time, 
        TimeSpan timeout)
    {
        var utcTime = time.ToUniversalTime();
        var utcNow = DateTime.UtcNow;

        return utcNow - utcTime > timeout;
    }
}