using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TimeFilter
{
    public short TimeStart;
    public short TimeEnd;
    public ModableStringList Days;

    public bool isValid(DateTime dateTime)
    {
        int time = dateTime.Minute + dateTime.Hour * 100;
        int day = (int)dateTime.DayOfWeek;

        if (time < TimeStart || time > TimeEnd)
            return false;

        if (Days == null)
            return true;

        if (Days.Count == 0)
            return false;

        switch (dateTime.DayOfWeek)
        {
            case (DayOfWeek.Monday):
                if (Days.Contains("Mo")) return true;
                return false;
            case (DayOfWeek.Tuesday):
                if (Days.Contains("Tu")) return true;
                return false;
            case (DayOfWeek.Wednesday):
                if (Days.Contains("We")) return true;
                return false;
            case (DayOfWeek.Thursday):
                if (Days.Contains("Th")) return true;
                return false;
            case (DayOfWeek.Friday):
                if (Days.Contains("Fr")) return true;
                return false;
            case (DayOfWeek.Saturday):
                if (Days.Contains("Sa")) return true;
                return false;
            case (DayOfWeek.Sunday):
                if (Days.Contains("Su")) return true;
                return false;
        }

        return false;
    }
}

public class TimeFilters : ModableDictionary<TimeFilter>, IModable
{
    public bool isValid(DateTime dateTime)
    {
        foreach(TimeFilter filter in Values)
        {
            if (filter.isValid(dateTime))
                return true;
        }
        return false;
    }

    public new IModable copyDeep()
    {
        return base.copyDeep<TimeFilters>();
    }
}

