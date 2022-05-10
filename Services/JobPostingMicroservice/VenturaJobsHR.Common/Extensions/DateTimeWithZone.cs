namespace VenturaJobsHR.Common.Extensions;

public readonly struct DateTimeWithZone
{
    public DateTimeWithZone(DateTime date)
    {
        var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        UniversalTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, TimeZone);
    }

    private DateTime UniversalTime { get; }
    private TimeZoneInfo TimeZone { get; }
    public DateTime LocalTime => TimeZoneInfo.ConvertTime(UniversalTime, TimeZone);
}
