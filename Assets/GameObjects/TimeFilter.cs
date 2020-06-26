using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Modable(ModableAttribute.FieldOptions.OptOut)]
public class TimeFilter : IModable, IModableAutofields
{
    public int TimeStart=0;
    public int TimeEnd =2359;
    public ModableStringList Days;

    [JsonProperty("Activity")]
    public string _activityRaw;

    [JsonIgnore]
    public string Activity
    {
        get
        {
            if (String.IsNullOrEmpty(_activityRaw))
                return "idle";
            return _activityRaw;
        }
        set => _activityRaw = value;
    }

    [JsonProperty("Condition")]
    private string _conditionString;
    [JsonIgnore]
    public Condition Condition { get => GameManager.Instance.ConditionCache[_conditionString]; }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }

    public bool isValid(DateTime dateTime)
    {
        if (isValidDateTime(dateTime) && Condition.evaluate(GameManager.Instance.GameData))
            return true;
        return false;
    }

    public bool isValidDateTime(DateTime dateTime)
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

    public void mod(IModable modable)
    {
        throw new NotImplementedException();
    }
}

public class TimeFilters : ModableObjectHashDictionary<TimeFilter>, IModable
{
    public bool tryGetValid(DateTime dateTime, out TimeFilter timeFilter)
    {
        timeFilter = null;

        foreach (TimeFilter filter in Values)
        {
            if (filter.isValid(dateTime))
            {
                timeFilter = filter;
                return true;
            }
                
        }
        return false;
    }

    public bool isValid(DateTime dateTime)
    {
        if (Count == 0)
            return true;

        foreach(TimeFilter filter in Values)
        {
            if (filter.isValid(dateTime))
                return true;
        }
        return false;
    }

    public new IModable copyDeep()
    {
        return copyDeep(this);
    }
}

