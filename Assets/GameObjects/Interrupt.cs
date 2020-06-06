using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class Interrupt : IModable, IModableAutofields
{
    [JsonIgnore]
    public string id;

    public float Chance = 1f;
    public int Cooldown=0;

    public CommandsCollection Commands = new CommandsCollection();

    [JsonProperty("Condition")]
    public string _condition;
    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[_condition];
    }

    public ModableStringList Listen = new ModableStringList();

    public int Priority = 0;

    public static int ComparePriorities(Interrupt x, Interrupt y)
    {
        return y.Priority - x.Priority;
    }

    public bool conditionCheck()
    {
        if (Condition == null)
            return true;
        return Condition.evaluate(GameManager.Instance.GameData);
    }

    public void execute()
    {
        GameManager.Instance.GameData.Interrupts.resetCooldown(this, Cooldown);
        Commands.execute();
    }

    public bool trySelect()
    {
        float random = Random.Range(0f,1f);
        if (random > Chance)
            return false;
        if (GameManager.Instance.GameData.Interrupts.remainingCooldown(this) > 0)
            return false;
        return conditionCheck();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }
}

public class InterruptsFile : IModable
{
    public ModableObjectHashDictionary<Interrupt> interrupts;

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}