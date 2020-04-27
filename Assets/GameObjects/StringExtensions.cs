using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static dynamic ToValue(this String str, Data data)
    {
        if (str == "true")
            return true;


        if (str == "false")
            return false;

        if (str == "null")
            return null;

        if (str.Length >= 3 && str[0] == '"')
            return str.Substring(1, str.Length - 2);


        long l = 0;
        if (long.TryParse(str, out l))
            return l;

        decimal d = 0.0M;
        if (decimal.TryParse(str, out d))
            return d;

        return data[str];
    }
}
