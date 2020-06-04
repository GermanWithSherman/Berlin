using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTimePass : Command
{

    public string ActivityID = "";
    public int Duration = 0;
    public int ToTime = -1;
    public CommandTimePass() { }

    public CommandTimePass(int time)
    {
        Duration = time;
    }

    public CommandTimePass(int time, string activityId)
    {
        ActivityID = activityId;
        Duration = time;
    }

    public override void execute(Data data)
    {
        int duration = Duration;
        if (ToTime >= 0)
            duration = GameManager.Instance.timeSecondsTils(ToTime);

        int timeTilMidnight = GameManager.Instance.timeSecondsTils(0,false);

        if(duration > timeTilMidnight)
        {
            
            CommandTimePass timePassTilMidnight = new CommandTimePass(timeTilMidnight,ActivityID);
            CommandInterrupt dayStartInterrupt = new CommandInterrupt("dayStart");
            CommandTimePass timePassAfterMidnight = new CommandTimePass(duration-timeTilMidnight, ActivityID);

            CommandsCollection commands = new CommandsCollection() { timePassTilMidnight, dayStartInterrupt, timePassAfterMidnight };
            commands.execute();
        }
        else
        {
            GameManager.Instance.timePass(duration, ActivityID);
        }

        
        return;
    }

    public override IModable copyDeep()
    {
        var result = new CommandTimePass();

        result.ActivityID = Modable.copyDeep(ActivityID);
        result.Duration = Modable.copyDeep(Duration);
        result.ToTime = Modable.copyDeep(ToTime);
        return result;
    }

    private void mod(CommandTimePass original, CommandTimePass mod)
    {
        ActivityID = Modable.mod(original.ActivityID, mod.ActivityID);
        Duration = Modable.mod(original.Duration, mod.Duration);
        ToTime = Modable.mod(original.ToTime, mod.ToTime);

    }

    public void mod(CommandTimePass modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandTimePass modCommand = modable as CommandTimePass;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
