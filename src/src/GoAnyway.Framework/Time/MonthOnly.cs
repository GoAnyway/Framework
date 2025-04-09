using GoAnyway.Framework.Assertion;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace GoAnyway.Framework.Time;

public readonly struct MonthOnly : IEquatable<MonthOnly>, IComparable<MonthOnly>
{
    private readonly DateTimeKind _kind;

    public int Month { get; }
    public int Year { get; }

    private MonthOnly(
        int month, 
        int year,
        DateTimeKind kind)
    {
        Month = month;
        Year = year;
        _kind = kind.ThrowIfNotDefined();
    }

    public static MonthOnly Current()
    {
        return FromDateTime(DateTime.Now);
    }

    public static MonthOnly CurrentUtc()
    {
        return FromDateTime(DateTime.UtcNow);
    }

    public MonthOnly Next()
    {
        DateTime dateTime = this;
        return FromDateTime(dateTime.AddMonths(1));
    }

    public static MonthOnly FromDateTime(DateTime dateTime)
    {
        var month = dateTime.Month;
        var year = dateTime.Year;

        return new(
            month: month,
            year: year,
            kind: dateTime.Kind
        );
    }

    public override string ToString()
    {
        DateTime dateTime = this;
        return dateTime.ToString(CultureInfo.CurrentCulture);
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format)
    {
        DateTime dateTime = this;
        return dateTime.ToString(format);
    }

    public string ToString(IFormatProvider? provider)
    {
        DateTime dateTime = this;
        return dateTime.ToString(provider);
    }

    public string ToString(
        [StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format, 
        IFormatProvider? provider)
    {
        DateTime dateTime = this;
        return dateTime.ToString(format, provider);
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
        var dateTime = new DateTime(
            year: month.Year, 
            month: month.Month, 
            day: 1
        );

        return DateTime.SpecifyKind(dateTime, month._kind);
    }
}