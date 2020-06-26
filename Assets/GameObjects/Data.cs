﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Data
{
    public dynamic this[string key]
    {
        get => getOrSpecial(key);
        set => set(key, value);
    }

    protected dynamic getOrSpecial(string key)
    {
        if (String.IsNullOrEmpty(key))
            return "";

        if (key[0] == '"' && key.Length >= 2)
        {
            return key.Substring(1, key.Length-2);
        }

        int bracketOpenIndex = key.IndexOf('(',1);
        int bracketCloseIndex = key.LastIndexOf(')');

        if(bracketOpenIndex >= 0 && bracketCloseIndex >= 0 && bracketOpenIndex < bracketCloseIndex)
        {
            string functionName = key.Substring(0,bracketOpenIndex);
            string parameters = key.Substring(bracketOpenIndex+1, bracketCloseIndex- bracketOpenIndex-1);
            FunctionParameters functionParameters = new FunctionParameters(parameters);
            dynamic result = GameManager.Instance.FunctionsLibrary.FunctionExecute(functionName,functionParameters);
            Debug.Log($"Call function {functionName} with {parameters} => {result}");
            return result;
        }

        return get(key);
    }

    protected abstract dynamic get(string key);

    protected abstract void set(string key, dynamic value);

    public virtual bool tryGetTypedValue<T>(string key, out T value)
    {
        value = default(T);
        if (!tryGetValue(key, out dynamic dynResult))
            return false;
        if (!(dynResult is T))
            return false;
        value = dynResult;
        return true;
    }

    public virtual bool tryGetValue(string key, out dynamic result)
    {
        result = getOrSpecial(key);
        

        if (result is null)
            return false;

        if (result is string && String.IsNullOrEmpty(result))
            return false;


        return true;
    }

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

    protected bool parseBool(dynamic original)
    {
        /*if (original is bool)
            return original;

        if (original is string)
            if(Boolean.TryParse(original, out bool result))
                return result;
        return false;*/
        try
        {
            return Convert.ToBoolean(original);
        }
        catch
        {
            return false;
        }

    }

    protected int parseInt(dynamic original)
    {
        try
        {
            return Convert.ToInt32(original);
        }
        catch
        {
            return 0;
        }
    }

    protected string parseString(dynamic original)
    {
        return Convert.ToString(original);
    }
}
