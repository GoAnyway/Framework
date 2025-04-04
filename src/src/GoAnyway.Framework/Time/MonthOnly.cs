namespace GoAnyway.Framework.Time;

public readonly struct MonthOnly : IEquatable<MonthOnly>, IComparable<MonthOnly>
{
    public int Month { get; }
    public int Year { get; }

    public MonthOnly(int month, int year)
    {
        Month = month;
        Year = year;
    }

    public static MonthOnly Current()
    {
        var now = DateTime.Now;
        return new(
            now.Month,
            now.Year
        );
    }

    public static MonthOnly FromDateTime(DateTime dateTime)
    {
        var month = dateTime.Month;
        var year = dateTime.Year;

        return new(
            month: month,
            year: year
        );
    }

    public override string ToString()
    {
        return ((DateTime)this).ToString("MMMM yyyy");
    }

    public bool Equals(MonthOnly other)
    {
        return Year == other.Year &&
               Month == other.Month;
    }

    public override bool Equals(object? obj)
    {
        return obj is MonthOnly other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Year, Month);
    }

    public static bool operator ==(MonthOnly left, MonthOnly right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MonthOnly left, MonthOnly right)
    {
        return !left.Equals(right);
    }

    public int CompareTo(MonthOnly other)
    {
        var yearComparsion = Year.CompareTo(other.Year);
        if (yearComparsion != 0)
            return yearComparsion;

        return Month.CompareTo(other.Month);
    }

    public static bool operator <(MonthOnly left, MonthOnly right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(MonthOnly left, MonthOnly right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(MonthOnly left, MonthOnly right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(MonthOnly left, MonthOnly right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static implicit operator DateTime(MonthOnly month)
    {
        return new(
            year: month.Year, 
            month: month.Month, 
            day: 1
        );
    }
}