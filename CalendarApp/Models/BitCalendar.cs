using System.Numerics;

namespace CalendarApp.Models;

public class BitCalendar
{
    private const int Days = 366;
    private const int Blocks = 6;

    private readonly ulong[] _data = new ulong[Blocks];
    private readonly int[] _blockPrefix = new int[Blocks + 1];
    public int TotalWorkingDays => _blockPrefix[Blocks];

    public int Year { get; set; }
    public BitCalendar(Schedule schedule, int year)
    {
        Year = year;

        var dayCount = DateTime.IsLeapYear(year) 
            ? Days 
            : 365;
        var date = new DateTime(year, 1, 1)
            .AddDays(-1);
        
        foreach (var dayNumber in Enumerable.Range(0, dayCount-1))
        {
            date = date.AddDays(1);
            if(schedule.IsDateWorking(date))
                SetWorkingDay(dayNumber);
        }
        
        BuildIndex();
    }

    private void SetWorkingDay(int dayIndex)
    {
        var block = dayIndex >> 6;
        var offset = dayIndex & 63;
        _data[block] |= 1UL << offset;
    }

    private void BuildIndex()
    {
        _blockPrefix[0] = 0;
        for (var i = 0; i < Blocks; i++)
        {
            _blockPrefix[i + 1] =
                _blockPrefix[i] + BitOperations.PopCount(_data[i]);
        }
    }
    
    public bool IsWorkingDay(int dayIndex)
    {
        dayIndex--;
        var block = dayIndex >> 6;
        var offset = dayIndex & 63;

        return (_data[block] & (1UL << offset)) != 0;
    }
    
    public int RemainingAfter(int dayIndex)
    {
        return TotalWorkingDays - GetRank(dayIndex);
    }

    public int FirstWorkingDay()
    {
        return NextWorkingDay(1);
    }

    public int NextWorkingDay(int n)
    {
        if (n <= 0 || n > TotalWorkingDays)
            throw new ArgumentOutOfRangeException(nameof(n));
        
        var block = 0;
        while (_blockPrefix[block + 1] < n)
            block++;
        
        var rankInsideBlock = n - _blockPrefix[block];
        
        var bits = _data[block];
        var bitIndex = SelectBit(bits, rankInsideBlock);

        return (block << 6) + bitIndex;
    }

    public int AddWorkingDays(int dayIndex, int n)
    {
        var currentRank = GetRank(dayIndex);
        var targetRank = currentRank + n;

        if (targetRank <= 0 || targetRank > _blockPrefix[Blocks])
            throw new ArgumentOutOfRangeException(nameof(n));
        
        var block = 0;
        while (_blockPrefix[block + 1] < targetRank)
            block++;

        var rankInsideBlock = targetRank - _blockPrefix[block];
        var bits = _data[block];
        var bitIndex = SelectBit(bits, rankInsideBlock);

        return (block << 6) + bitIndex;
    }

    private int GetRank(int dayIndex)
    {
        var block = dayIndex >> 6;
        var offset = dayIndex & 63;

        var mask = (offset == 63)
            ? ulong.MaxValue
            : (1UL << (offset + 1)) - 1;

        var rank =
            _blockPrefix[block] +
            BitOperations.PopCount(_data[block] & mask);

        return rank;
    }
    
    private static int SelectBit(ulong bits, int k)
    {
        var count = 0;

        while (true)
        {
            var tz = BitOperations.TrailingZeroCount(bits);
            bits &= bits - 1; 
            count++;

            if (count == k)
                return tz;
        }
    }
}