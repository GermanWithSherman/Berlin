using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEvent : Command
{
    [JsonProperty("Event")]
    private EventStage _event;


    [JsonIgnore]
    public EventStage Event
    {
        get
        {
            if (_event == null)
                return null;
            if (!_event.isInheritanceResolved())
                _event.inherit();
            return _event;
        }
    }

    public string EventID = "";
    public string EventGroup = "";
    public string EventStage = "";


    public override void execute(Data data)
    {
        if (Event != null)
        {
            GameManager.Instance.eventExecute(Event);
        }
        else if (!String.IsNullOrEmpty(EventID))
        {
            GameManager.Instance.eventExecute(EventID);
        }
        else
        {
            GameManager.Instance.eventExecute(EventGroup, EventStage);
        }
    }
}
