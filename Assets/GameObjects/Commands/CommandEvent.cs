using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEvent : Command
{
    public string EventID = "";
    public string EventGroup = "";
    public string EventStage = "";


    public override void execute(Data data)
    {
        if (!String.IsNullOrEmpty(EventID))
        {
            GameManager.Instance.eventExecute(EventID);
        }
        else
        {
            GameManager.Instance.eventExecute(EventGroup, EventStage);
        }
    }
}
