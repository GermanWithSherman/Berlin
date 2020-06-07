using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc
{
    public LookupTable outfitStyle = new LookupTable("Weird");
    public ModableValueTypeHashDictionary<string[]> dayNightCycle;

    public string dayNightState(DateTime dateTime)
    {
        string month = dateTime.Month.ToString();
        if(dayNightCycle.TryGetValue(month,out string[] monthlyDayNightCycle))
        {
            int hour = dateTime.Hour;
            if(hour >= monthlyDayNightCycle.Length)
            {
                Debug.LogError($"Error reading dayNightCycle for hour {hour} in month {month}");
                return "day";
            }
            return monthlyDayNightCycle[hour];
        }
        Debug.LogError($"Error reading dayNightCycle for month {month}");
        return "day";
    }
}


public class LookupTable
{
    public string Default;
    public ModableObjectSortedDictionary<ModableValueTypeSortedDictionary<string>> Values;

    public LookupTable(string def)
    {
        Default = def;
    }

    public string get(IEnumerable<string> keySet1, IEnumerable<string> keySet2)
    {
        foreach (string key1 in keySet1)
        {
            if(Values.TryGetValue(key1,out ModableValueTypeSortedDictionary<string> InnerValues))
            {
                foreach (string key2 in keySet2)
                {
                    if (InnerValues.TryGetValue(key2, out string ResultValue))
                    {
                        if (ResultValue != Default)
                            return ResultValue;
                    }
                }
            }
        }

        return Default;
    }
}