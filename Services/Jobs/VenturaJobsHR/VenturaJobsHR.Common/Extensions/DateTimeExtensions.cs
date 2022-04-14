namespace VenturaJobsHR.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ConvertDateTime(DateTime date)
    {
        var dateTimeUnspec = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        var convertedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeUnspec, "E. South America Standard Time");

        return convertedDate;
    }
}
