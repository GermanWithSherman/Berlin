using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSet : Command
{
    public ModableValueTypeSortedDictionary<dynamic> Values = new ModableValueTypeSortedDictionary<dynamic>();
    public ModableValueTypeSortedDictionary<dynamic> ValuesFromLists = new ModableValueTypeSortedDictionary<dynamic>();
    public ModableObjectSortedDictionary<ValueSetter> ValuesFromSetters = new ModableObjectSortedDictionary<ValueSetter>();
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
            dynamic target = kv.Value.Target(data[kv.Key]);
            data[kv.Key] = target;
            Debug.Log($"{kv.Key} => {target}");
        }
    }

    public override IModable copyDeep()
    {
        var result = new CommandSet();

        result.Values = Modable.copyDeep(Values);
        result.ValuesFromLists = Modable.copyDeep(ValuesFromLists);
        result.ValuesFromSetters = Modable.copyDeep(ValuesFromSetters);
        return result;
    }

    private void mod(CommandSet original, CommandSet mod)
    {
        Values = Modable.mod(original.Values, mod.Values);
        ValuesFromLists = Modable.mod(original.ValuesFromLists, mod.ValuesFromLists);
        ValuesFromSetters = Modable.mod(original.ValuesFromSetters, mod.ValuesFromSetters);

    }

    public void mod(CommandSet modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandSet modCommand = modable as CommandSet;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}

public class ValueSetter: IModable
{
    public enum SetMode { Set, Inc, Cooldown, Object }

    [JsonConverter(typeof(StringEnumConverter))]
    public SetMode Mode;

    public dynamic Value;

    public Value<dynamic> ValueObject;

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
            case (SetMode.Object):
                return ValueObject.value();
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

        if(Value != null)
            result.Value = Modable.copyDeep(Value);

        result.ValueObject = Modable.copyDeep(ValueObject);

        return result;
    }
}