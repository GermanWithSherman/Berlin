using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Conditional : IModable
{
    public abstract IModable copyDeep();

    public abstract void mod(IModable modable);

    public static Conditional<T> mod<T>(Conditional<T> original, Conditional mod)
    {
        if (mod == null)
            return original;

        if(!(mod is Conditional<T>))
        {
            Debug.LogError("Type mismatch");
            return original;
        }

        if (original == null)
            return ((Conditional<T>)mod.copyDeep());


        original.mod(mod);

        return original;
    }

}

public class Conditional<T> : Conditional
{
    public enum ConditionalMode { Default, Enum, Random }

    [JsonProperty]
    private T Value = default(T);

    public ModableDictionary<Value<T>> Values = new ModableDictionary<Value<T>>();

    [JsonConverter(typeof(StringEnumConverter))]
    public ConditionalMode Mode = ConditionalMode.Default;

    public static implicit operator T(Conditional<T> conditional)
    {
        if (conditional == null)
            return default(T);
        return conditional.value();
    }

    public Conditional() { }

    public Conditional(T v,int p=0,bool singleValue = false){
        if (singleValue)
            Value = v;
        else
            Values.Add("#def", new Value<T>(v, p));
    }

    public T value()
    {
        GameManager gameManager = GameManager.Instance;
        GameData gameData = gameManager.GameData;
        return value(gameData);
    }


    public T value(Data data) { 


        if (Value != null && !Value.Equals(default(T)))
            return Value;

        switch (Mode)
        {
            case (ConditionalMode.Default):
                List<Value<T>> values = Values.Values.ToList();
                values.Sort(Value<T>.ComparePriorities);


                foreach (Value<T> value in values)
                {
                    if (value.Condition.evaluate(data))
                        return value.value(data);
                }

                Debug.Log("No valid value, returning default");

                return default;
            case (ConditionalMode.Enum):
                string index = (string)data["KEY"];
                return Values[index].value(data);
            case (ConditionalMode.Random):
                List<Value<T>> possibleValues = Values.Values.ToList();
                List<Value<T>> validValues = new List<Value<T>>();

                foreach (Value<T> value in possibleValues)
                {
                    if (value.Condition.evaluate(data))
                        validValues.Add( value);
                }

                if (validValues.Count > 0)
                {
                    return validValues.GetRandom();
                }
                
                Debug.Log("No valid value, returning default");

                return default;

        }

        return default;
    }

    public IEnumerable<T> values()
    {
        GameManager gameManager = GameManager.Instance;
        GameData gameData = gameManager.GameData;
        return values(gameData);
    }

    public IEnumerable<T> values(Data data)
    {
        var result = new List<T>();

        switch (Mode)
        {
            case (ConditionalMode.Default):
                
                List<Value<T>> values = Values.Values.ToList();
                                
                values.Sort(Value<T>.ComparePriorities);


                foreach (Value<T> value in values)
                {
                    if (value.Condition.evaluate(data))
                        result.Add( value.value(data));
                }

                if (Value != null)
                    result.Add(Value);

                return result;
            case (ConditionalMode.Random):
                List<Value<T>> ranvalues = Values.Values.ToList();
                List<T> validValues = new List<T>();

                foreach (Value<T> value in ranvalues)
                {
                    if (value.Condition.evaluate(data))
                        validValues.Add(value.value(data));
                }

                if (Value != null)
                    validValues.Add(Value);

                if(validValues.Count > 0)
                    result = new List<T>() { validValues.GetRandom() };

                return result;

        }
        return result;
    }

    

    public void mod(Conditional<T> modable)
    {
        if (modable == null) return;
        Value = (modable.Value != null && !modable.Value.Equals(default(T))) ? modable.Value : Value;
        Values.mod(modable.Values);
    }

    public override void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((Conditional<T>)modable);
    }

    public override IModable copyDeep()
    {
        var result = new Conditional<T>();
        result.Mode = Mode;
        result.Value = Value;
        result.Values = Modable.copyDeep(Values);//(ModableDictionary<Value<T>>)Values.copyDeep();

        return result;
    }
}
