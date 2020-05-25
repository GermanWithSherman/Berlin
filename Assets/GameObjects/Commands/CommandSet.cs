using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSet : Command
{
    public ModableDictionary<dynamic> Values = new ModableDictionary<dynamic>();
    public ModableDictionary<dynamic> ValuesFromLists = new ModableDictionary<dynamic>();
    public ModableDictionary<ValueSetter> ValuesFromSetters = new ModableDictionary<ValueSetter>();
    public override void execute(Data data)
    {
        foreach(KeyValuePair<string,dynamic> kv in Values)
        {
            data[kv.Key] = kv.Value;
            Debug.Log($"{kv.Key} => {kv.Value}");
        }

        foreach (KeyValuePair<string, dynamic> kv in ValuesFromLists)
        {
            data[kv.Key] = GameManager.Instance.WeightedStringListCache[kv.Value].value();
        }

        foreach (KeyValuePair<string, ValueSetter> kv in ValuesFromSetters)
        {
            data[kv.Key] = kv.Value.Target(data[kv.Key]);
        }
    }
}

public class ValueSetter: IModable
{
    public enum SetMode { Set, Inc, Cooldown }

    [JsonConverter(typeof(StringEnumConverter))]
    public SetMode Mode;

    public dynamic Value;

    private dynamic modeInc(dynamic currentValue) => (int)currentValue + (int)Value;

    public dynamic Target(dynamic currentValue)
    {
        switch (Mode)
        {
            case (SetMode.Cooldown):
                DateTime now = GameManager.Instance.GameData.WorldData.DateTime;
                TimeSpan cooldown = new TimeSpan(0, 0, (int)Value);
                return now + cooldown;
            case (SetMode.Inc):
                return modeInc(currentValue);
            case (SetMode.Set):
                return Value;
        }
        return null;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public IModable copyDeep()
    {
        var result = new ValueSetter();
        result.Mode = Mode;
        result.Value = Modable.copyDeep(Value);
        return result;
    }
}