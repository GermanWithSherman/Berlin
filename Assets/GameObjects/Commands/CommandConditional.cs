using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConditional : Command
{
    public CommandsCollection Commands;

    [JsonProperty("Condition")]
    public string ConditionString = "";

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[ConditionString];
    }

    public override void execute(Data data)
    {
        if (Condition.evaluate(data))
            Commands.execute(data);
    }
}
