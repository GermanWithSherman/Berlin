using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RandomRange
{
    private float _min = 0f;
    private float _max = 1f;

    //private bool _lowInclusive = true;
    //private bool _highInclusive = true;

    public RandomRange(IDictionary<string,string> settings)
    {
        if (settings.ContainsKey("Min"))
            _min = Convert.ToSingle(settings["Min"]);
        if (settings.ContainsKey("Max"))
            _max = Convert.ToSingle(settings["Max"]);
        /*if (settings.ContainsKey("LowInclusive"))
            _lowInclusive = Convert.ToBoolean(settings["LowInclusive"]);
        if (settings.ContainsKey("HighInclusive"))
            _highInclusive = Convert.ToBoolean(settings["HighInclusive"]);*/
    }

    public float getFloat()
    {
        return UnityEngine.Random.Range(_min,_max);
    }

    public float? getFloatNullable()
    {
        return UnityEngine.Random.Range(_min, _max);
    }

    public int getInt()
    {
        return UnityEngine.Random.Range((int)_min, (int)_max);
    }

    public int? getIntNullable()
    {
        return UnityEngine.Random.Range((int)_min, (int)_max);
    }
}

