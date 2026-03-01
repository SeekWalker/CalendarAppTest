namespace CalendarApp.Models;

public class Schedule
{
    public ICollection<ScheduledDay> ScheduledDays { get; set; } = [];
    
    public DateTime GetNextDay(DateTime date, int duration)
    {
        var currentDate = date;
        while (duration>0)
        {
            if(IsDateWorking(currentDate))
                duration--;

            currentDate = currentDate.AddDays(1);
        }
        return currentDate;
    }
    
    public bool IsDateWorking(DateTime date)
    {
        return ScheduledDays.LastOrDefault(x=>x.IsDateMatch(date))?.IsWorking ?? true;
    }
}