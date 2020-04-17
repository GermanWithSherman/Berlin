using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupt
{
    [JsonIgnore]
    public string id;

    public float chance = 1f;

    public CommandsCollection Commands = new CommandsCollection();

    public Condition condition;
    public List<string> listen = new List<string>();

    public int priority = 0;

    public static int ComparePriorities(Interrupt x, Interrupt y)
    {
        return y.priority - x.priority;
    }

    public bool conditionCheck()
    {
        if (condition == null)
            return true;
        return condition.evaluate(GameManager.Instance.GameData);
    }

    public void execute()
    {
        Commands.execute();
    }

    public bool trySelect()
    {
        float random = Random.Range(0f,1f);
        if (random > chance)
            return false;
        return conditionCheck();
    }
}
