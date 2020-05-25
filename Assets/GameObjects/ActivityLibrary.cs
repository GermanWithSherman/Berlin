using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityLibrary : Library<Activity>
{
    public Activity this[string key]
    {
        get
        {
            if (_dict.TryGetValue(key, out Activity activity))
                return activity;
            return _defaultValue;
        }
    }

    private Activity _defaultValue
    {
        get
        {
            return _dict["default"];
        }
    }

    public ActivityLibrary()
    {
        Activity defaultActivity = new Activity();
        defaultActivity.statHunger = -23;
        defaultActivity.statHygiene = -6;
        defaultActivity.statSleep = -11;

        Activity sleep = new Activity();
        sleep.statHunger = defaultActivity.statHunger / 2;
        sleep.statHygiene = -6;
        sleep.statSleep = defaultActivity.statSleep * -2;


        _dict.Add("default", defaultActivity);
        _dict.Add("sleep", sleep);
    }
}
