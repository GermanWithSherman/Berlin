﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTimePass : Command
{

    public string ActivityId = "";
    public int Duration = 0;

    public CommandTimePass() { }

    public CommandTimePass(int time)
    {
        Duration = time;
    }

    public CommandTimePass(int time, string activityId)
    {
        ActivityId = activityId;
        Duration = time;
    }

    public override void execute(Data data)
    {
        GameManager.Instance.timePass(Duration, ActivityId);
        return;
    }
}