using CalendarApp.Models;

namespace CalendarApp.Extensions;

public static class BitmapCalendarExtensions
{
    public static bool IsWorkingDay(this BitCalendar calendar, DateTime date)
    {
        var dayOfYear = date.DayOfYear;
        
        return calendar.IsWorkingDay(dayOfYear);
    }
    public static DateTime GetEndDate(this BitCalendar calendar, DateTime date, int duration)
    {
        var dayOfYear = date.DayOfYear;
        
        if (calendar.IsWorkingDay(date) && duration == 0)
            return date;

        var dayNumber = calendar.AddWorkingDays(dayOfYear, duration);
        return new DateTime(calendar.Year, 1,1).AddDays(dayNumber);
    }
    
    public static DateTime GetEndDate(this MultiYearBitCalendar calendar, DateTime date, int duration)
    {
        var dayOfYear = date.DayOfYear;

        if (calendar.IsWorkingDay(date) && duration == 0)
            return date;

        var dateInfo = calendar.AddWorkingDays(date.Year,dayOfYear, duration);
        
        return new DateTime(dateInfo.year, 1,1).AddDays(dateInfo.dayIndex);
    }
}