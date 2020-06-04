using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData:Data
{
    public DateTime DateTime = new DateTime(2000,1,1);

    [JsonIgnore]
    public string DayPhase
    {
        get => dayPhase();
    }

    
    private string dayPhase()
    {
        /*if(DateTime.Hour >= 22 || DateTime.Hour < 6)
        {
            return "night";
        }
        return "day";*/

        return GameManager.Instance.Misc.dayNightState(DateTime);
    }

    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "DateTime":
                return DateTime;
            case "DayPhase":
                return DayPhase;
        }
        return "";
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "DateTime":
                DateTime = Convert.ToDateTime(value);
                break;
        }
    }
}
