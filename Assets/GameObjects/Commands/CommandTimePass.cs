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

        GameManager.Instance.timePass(duration, ActivityID);
        return;
    }
}
