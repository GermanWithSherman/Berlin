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

        /*if (original.GetType() != mod.GetType())
        {
            Debug.LogError("Type mismatch");
            return original;
        }*/

        original.mod(mod);

        return original;
    }

}

public class Conditional<T> : Conditional
{
    public enum ConditionalMode { Default, Enum }

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

        }

        return default;
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

        result.Value = Value;
        result.Values = Modable.copyDeep(Values);//(ModableDictionary<Value<T>>)Values.copyDeep();

        return result;
    }
}
