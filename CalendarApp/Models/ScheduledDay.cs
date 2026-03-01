using CalendarApp.Enums;
using CalendarApp.Extensions;

namespace CalendarApp.Models;

public class ScheduledDay
{
    public RepeatDayType DayType { get; set; }
    public DateOnly SettingDate { get; set; }
    public bool IsWorking { get; set; }

    public bool IsDateMatch(DateTime date)
    {
        return DayType switch {
            RepeatDayType.Single => date.Date.ToDateOnly() == SettingDate,
            RepeatDayType.Daily => true,
            RepeatDayType.Weekly => date.DayOfWeek == SettingDate.DayOfWeek,
            RepeatDayType.Monthly => date.Day == SettingDate.Day 
                                     && date.Month == SettingDate.Month,
            RepeatDayType.Yearly => date.Day == SettingDate.Day 
                                    && date.Month == SettingDate.Month,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}