﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Data
{
    public dynamic this[string key]
    {
        get => get(key);
        set => set(key, value);
    }

    protected abstract dynamic get(string key);

    protected abstract void set(string key, dynamic value);

    public string interpolate(string template)
    {
        string result = "";

        int maxRounds = 1000; //TODO: Test
        int startIndex = 0;
        int endIndex = 0;
        while (startIndex >= 0)
        {
            startIndex = template.IndexOf('{', endIndex);

            if (startIndex >= 0)
            {
                result += template.Substring(endIndex, startIndex - endIndex);
                endIndex = template.IndexOf('}', startIndex);
                if (endIndex < (startIndex-1))
                    throw new System.Exception("Interpolate String malformed");
                string key = template.Substring(startIndex + 1, endIndex - startIndex - 1);
                result += (string)get(key);
                endIndex++; //so we don't catch { above
            }
            else
            {
                result += template.Substring(endIndex);
                break;
            }


            if (--maxRounds <= 0)
                throw new System.Exception("Max Rounds reached");
        }

        return result;
    }
}
