using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc
{
    public ModableDictionary<string[]> dayNightCycle;

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
