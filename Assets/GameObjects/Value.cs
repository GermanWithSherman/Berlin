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

public class Value<T>: Value, IModable
{
    public enum ValueTypes
    {
        None,
        Function,
        Interpolate,
        Plain,
        Reference,
        WeightedStringList,
        RandomRange
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public ValueTypes ValueType = ValueTypes.None;

    public ModableValueTypeHashDictionary<string> Parameters = new ModableValueTypeHashDictionary<string>();

    public string Key; //Key for Reference and WeightedStringList and Function

    [JsonProperty("Value")]
    private T _value; // value

    [JsonProperty("Priority")]
    private int? _priority;

    [JsonIgnore]
    public int Priority
    {
        get => _priority.GetValueOrDefault(0);
    }

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[_condition];
    }

    [JsonProperty("Condition")]
    public string _condition = ""; //condition String

    public Conditional<T> ConditionalValue; //conditional value

    public Value() { }

    public Value(T v, int p = 0)
    {
        _value = v;
        _priority = p;
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
        switch (ValueType)
        {
            case ValueTypes.Function:
                FunctionParameters p = new FunctionParameters();
                foreach(KeyValuePair<string,string> kv in Parameters)
                {
                    p.Add(kv.Key, kv.Value.ToValue(gameData));
                }
                return (T)(object)GameManager.Instance.FunctionsLibrary.functionExecute(Key,p);
            case ValueTypes.Interpolate:
                return (T)(object)gameData.interpolate(Key);
            case ValueTypes.None:
            case ValueTypes.Plain:
                if (ConditionalValue != null)
                    return ConditionalValue.value(gameData);
                return _value;
            case ValueTypes.Reference:
                if (String.IsNullOrEmpty(Key))
                    throw new KeyNotFoundException("Parameter Key missing");
                return gameData[Key];
            case ValueTypes.WeightedStringList:
                if(typeof(T) == typeof(System.String))
                    //return (T)(object)gameManager.WeightedStringListCache[K].value();
                    return (T)(object)GameManager.Instance.WeightedStringListCache[Key].value();
                break;
            case ValueTypes.RandomRange:
                if (typeof(int) == typeof(T) || typeof(int?) == typeof(T))
                    return (T)Convert.ChangeType(new RandomRange(Parameters).getIntNullable(), typeof(int));
                if (typeof(float) == typeof(T) || typeof(float?) == typeof(T))
                    return (T)Convert.ChangeType(new RandomRange(Parameters).getFloatNullable(), typeof(float));
                throw new GameException($"RandomRange is not a valid Value Type for Value<{typeof(T)}>");
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
        ValueType = mod.ValueType == ValueTypes.None ? original.ValueType : mod.ValueType;//mod.VT; //Modable.mod(original.VT, mod.VT);
        Parameters = Modable.mod(original.Parameters, mod.Parameters);

        if (original._value is IModable && original._value.GetType() == mod._value.GetType())
            _value = (T)Modable.mod((IModable)original._value, (IModable)mod._value);
        else
        {
            if(original._value is int)
                original._value = mod._value;
            else if(original._value is int?)
                original._value = mod._value == null ? original._value : mod._value;
        }
        Key = Modable.mod(original.Key, mod.Key);
        _priority = Modable.mod(original._priority, mod._priority);
        _condition = Modable.mod(original._condition, mod._condition);
        ConditionalValue = Modable.mod(original.ConditionalValue, mod.ConditionalValue);
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

        result._condition = _condition;
        result.ConditionalValue = Modable.copyDeep(ConditionalValue);
        result.Key = Modable.copyDeep(Key);
        result._priority = Modable.copyDeep(_priority);
        result.Parameters = Modable.copyDeep(Parameters);

        if (_value is IModable)
        {
            IModable VCopy = Modable.copyDeep((IModable)_value);
            result._value = (T)VCopy;
        }
        else
            result._value = _value;

        result.ValueType = ValueType;

        return result;
    }
}


