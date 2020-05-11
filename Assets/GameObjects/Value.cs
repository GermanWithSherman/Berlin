using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Value : IModable
{
    public abstract IModable copyDeep();
    public abstract void mod(IModable modable);
}

public class Value<T>: Value
{
    public enum ValueType
    {
        None,
        Function,
        Interpolate,
        Plain,
        Reference,
        WeightedStringList
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public ValueType VT = ValueType.None;

    //public Dictionary<string, string> Parameters = new Dictionary<string, string>(); //for ValueType.Function
    public ModableDictionary<string> Parameters = new ModableDictionary<string>();

    public string K; //Key for Reference and WeightedStringList and Function

    public T V; // value

    public int? P;

    [JsonIgnore]
    public int Priority
    {
        get => P.GetValueOrDefault(0);
    }

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
        if (value == null)
            return default;
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
            case ValueType.Function:
                FunctionParameters p = new FunctionParameters();
                foreach(KeyValuePair<string,string> kv in Parameters)
                {
                    p.Add(kv.Key, kv.Value.ToValue(gameData));
                }
                return (T)(object)GameManager.Instance.FunctionsLibrary.functionExecute(K,p);
            case ValueType.Interpolate:
                return (T)(object)gameData.interpolate(K);
            case ValueType.None:
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
        return y.Priority - x.Priority;
    }

    public void mod(Value<T> modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    private void mod(Value<T> original, Value<T> mod) {
        VT = mod.VT == ValueType.None ? original.VT : mod.VT;//mod.VT; //Modable.mod(original.VT, mod.VT);
        Parameters = Modable.mod(original.Parameters, mod.Parameters);

        if (original.V is IModable && original.V.GetType() == mod.V.GetType())
            V = (T)Modable.mod((IModable)original.V, (IModable)mod.V);
        else
        {
            if(original.V is int)
                original.V = mod.V;
            else if(original.V is int?)
                original.V = mod.V == null ? original.V : mod.V;
        }
        K = Modable.mod(original.K, mod.K);
        P = Modable.mod(original.P, mod.P);
        C = Modable.mod(original.C, mod.C);
        CV = Modable.mod(original.CV, mod.CV);
    }

    public override void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((Value<T>)modable);
    }

    public override IModable copyDeep()
    {
        var result = new Value<T>();

        result.C = C;
        result.CV = Modable.copyDeep(CV);
        result.K = K;
        result.P = P;

        if (V is IModable)
        {
            IModable VCopy = Modable.copyDeep((IModable)V);
            result.V = (T)VCopy;
        }
        else
            result.V = V;

        result.VT = VT;

        return result;
    }
}
