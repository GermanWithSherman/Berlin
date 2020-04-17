using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedules : Dictionary<string,Schedule>
{
    public bool isScheduled(DateTime dateTime)
    {
        if (Count == 0)
            return true;

        int time = dateTime.Minute + dateTime.Hour * 100;
        int day = (int)dateTime.DayOfWeek;

        foreach (Schedule schedule in Values)
        {
            if (schedule.d.Contains(day) && time >= schedule.start && time <= schedule.end)
            {
                return true;
            }
        }
        return false;
    }
}
