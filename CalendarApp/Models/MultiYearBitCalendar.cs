using CalendarApp.Extensions;

namespace CalendarApp.Models;

public class MultiYearBitCalendar
{
    private readonly Dictionary<int, BitCalendar> _years = new();
    private readonly Func<int, BitCalendar> _yearFactory;

    public MultiYearBitCalendar(Func<int, BitCalendar> yearFactory)
    {
        _yearFactory = yearFactory;
    }

    private BitCalendar GetYear(int year)
    {
        if (_years.TryGetValue(year, out var cal))
            return cal;

        cal = _yearFactory(year);
        _years[year] = cal;
        return cal;
    }
    
    public bool IsWorkingDay(DateTime date)
    {
        var year = GetYear(date.Year);
        
        return year.IsWorkingDay(date);
    }

    public (int year, int dayIndex) AddWorkingDays(
        int year,
        int dayIndex,
        int n)
    {
        if (n == 0)
            return (year, dayIndex);

        var cal = GetYear(year);

        var remaining = cal.RemainingAfter(dayIndex);

        // 1. помещается в текущем году
        if (n <= remaining)
        {
            var newDay = cal.AddWorkingDays(dayIndex, n);
            return (year, newDay);
        }

        // 2. выходим за пределы года
        n -= remaining;
        year++;

        while (true)
        {
            cal = GetYear(year);

            if (n <= cal.TotalWorkingDays)
            {
                var newDay = cal.NextWorkingDay(n);
                return (year, newDay);
            }

            n -= cal.TotalWorkingDays;
            year++;
        }
    }
}