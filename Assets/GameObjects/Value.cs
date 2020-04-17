﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Value<T>
{
    public enum ValueType
    {
        Interpolate,
        Plain,
        Reference,
        WeightedStringList
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public ValueType VT = ValueType.Plain;

    public string K; //Key for Reference and WSL

    public T V; // value

    public int P = 0;

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[C];
    }

    public string C = ""; //condition String

    public Conditional<T> CV; //conditional value

    public Value() { }

    public Value(T v, int p = 0)
    {
        V = v;
        P = p;
    }


    public static implicit operator T(Value<T> value)
    {
        return value.value();
    }

    public T value()
    {
        GameData gameData = GameManager.Instance.GameData;
        return value(gameData);
    }



    public T value(Data gameData)
    {
        switch (VT)
        {
            case ValueType.Interpolate:
                return (T)(object)gameData.interpolate(K);
            case ValueType.Plain:
                if (CV != null)
                    return CV.value(gameData);
                return V;
            case ValueType.Reference:
                if (String.IsNullOrEmpty(K))
                    throw new KeyNotFoundException("Parameter K missing");
                return gameData[K];
            case ValueType.WeightedStringList:
                if(typeof(T) == typeof(System.String))
                    //return (T)(object)gameManager.WeightedStringListCache[K].value();
                    return (T)(object)GameManager.Instance.WeightedStringListCache[K].value();
                break;
        }
        return default;
    }

    public static int ComparePriorities(Value<T> x, Value<T> y)
    {
        return y.P - x.P;
    }

}
