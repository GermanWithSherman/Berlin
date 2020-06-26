using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConditional : Command
{
    public CommandsCollection Commands;
    public CommandsCollection Else;


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
        else if (Else != null)
            Else.execute(data);
    }

    public override IModable copyDeep()
    {
        var result = new CommandConditional();

        result.Commands = Modable.copyDeep(Commands);
        result.ConditionString = Modable.copyDeep(ConditionString);
        result.Else = Modable.copyDeep(Else);

        return result;
    }

    private void mod(CommandConditional original, CommandConditional mod)
    {
        Commands = Modable.mod(original.Commands, mod.Commands);
        ConditionString = Modable.mod(original.ConditionString, mod.ConditionString);
        Else = Modable.mod(original.Else, mod.Else);

    }

    public void mod(CommandConditional modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandConditional commandConditional = modable as CommandConditional;
        if (commandConditional == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(commandConditional);
    }
}
