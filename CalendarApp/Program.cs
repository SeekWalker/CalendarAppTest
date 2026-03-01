// See https://aka.ms/new-console-template for more information

using CalendarApp.Enums;
using CalendarApp.Extensions;
using CalendarApp.Models;

var schedule = new Schedule()
{
    ScheduledDays = new List<ScheduledDay>()
    {
        new()
        {
            SettingDate = new DateOnly(2025,12,6),
            DayType = RepeatDayType.Weekly,
            IsWorking = false
        },
        new()
        {
            SettingDate = new DateOnly(2025,12,7),
            DayType = RepeatDayType.Weekly,
            IsWorking = false
        },
        new()
        {
            SettingDate = new DateOnly(2026,1,1),
            DayType = RepeatDayType.Yearly,
            IsWorking = false
        },
    }
};

var calendar = new BitCalendar(schedule, 2026);

Console.WriteLine(new DateTime(2026,2,27)
    .ToString()+calendar
    .IsWorkingDay(new DateTime(2026,2,27)));
Console.WriteLine(new DateTime(2026,2,28)
    .ToString()+calendar
    .IsWorkingDay(new DateTime(2026,2,28)));

var a = DateTime.Now;

foreach (var i in Enumerable.Range(1,230))
{
    Console.WriteLine(calendar.GetEndDate(new DateTime(2026,1,1), i));
}
Console.WriteLine(DateTime.Now-a);

var multi = new MultiYearBitCalendar(x=>new BitCalendar(schedule,x));

Console.WriteLine(new DateTime(2026,2,27)
    .ToString()+multi
    .IsWorkingDay(new DateTime(2026,2,27)));
Console.WriteLine(new DateTime(2026,2,28)
    .ToString()+multi
    .IsWorkingDay(new DateTime(2026,2,28)));

a = DateTime.Now;
foreach (var i in Enumerable.Range(1,10000))
{
    schedule.IsDateWorking(new DateTime(2026, 1, 1).AddDays(i));
}
    
var scheduleIsDateWorking = "schedule with iterator IsDateWorking"+ (DateTime.Now - a);

a = DateTime.Now;
foreach (var i in Enumerable.Range(1,10000))
{
    schedule.GetNextDay(new DateTime(2026, 1, 1),i);
}
    
var scheduleGetNextDay = "schedule with iterator GetNextDay"+ (DateTime.Now - a);

a = DateTime.Now;

foreach (var i in Enumerable.Range(1,10000))
{
    multi.IsWorkingDay(new DateTime(2026, 1, 1).AddDays(i));
}
    
var multiIsDateWorking = "multi bit calendar IsWorkingDay"+ (DateTime.Now - a);
a = DateTime.Now;

foreach (var i in Enumerable.Range(1,10000))
{
    multi.GetEndDate(new DateTime(2026, 1, 1), i);
}
var getDates = "multi bit calendar GetEndDate"+ (DateTime.Now - a);
a = DateTime.Now;
var listDates = new List<DateTime>();
foreach (var i in Enumerable.Range(1,10000))
{
    listDates.Add(multi.GetEndDate(new DateTime(2026,1,1), i));
}

var console ="Console.WriteLine multi bit calendar GetEndDate" + (DateTime.Now - a);

Console.WriteLine(scheduleIsDateWorking);
Console.WriteLine(scheduleGetNextDay);
Console.WriteLine(multiIsDateWorking);
Console.WriteLine(getDates);
Console.WriteLine(console);