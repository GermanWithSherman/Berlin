using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Conditional { }

public class Conditional<T> : Conditional
{
    [JsonProperty]
    private T Value = default(T);

    public Dictionary<string, Value<T>> Values = new Dictionary<string, Value<T>>();

    public static implicit operator T(Conditional<T> conditional)
    {
        if (conditional == null)
            return default(T);
        return conditional.value();
    }

    public Conditional() { }

    public Conditional(T v,int p=0){
        Values.Add("#def",new Value<T>(v,p));
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

        List<Value<T>> values = Values.Values.ToList();
        values.Sort(Value<T>.ComparePriorities);


        foreach (Value<T> value in values)
        {
            if (value.Condition.evaluate(data))
                return value.value(data);
        }

        Debug.Log("No valid value, returning default");

        return default;
    }
}
