using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTimePass : Command
{

    public string ActivityId = "";
    public int Time = 0;

    public CommandTimePass() { }

    public CommandTimePass(int time)
    {
        Time = time;
    }

    public CommandTimePass(int time, string activityId)
    {
        ActivityId = activityId;
        Time = time;
    }

    public override void execute(Data data)
    {
        GameManager.Instance.timePass(Time, ActivityId);
        return;
    }
}
