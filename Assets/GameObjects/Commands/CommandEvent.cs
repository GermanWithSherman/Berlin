using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEvent : Command
{
    [JsonProperty("Event")]
    private EventStage _event=null;


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

    public override IModable copyDeep()
    {
        var result = new CommandEvent();

        result.EventID = Modable.copyDeep(EventID);
        result.EventGroup = Modable.copyDeep(EventGroup);
        result.EventStage = Modable.copyDeep(EventStage);
        return result;
    }

    private void mod(CommandEvent original, CommandEvent mod)
    {
        EventID = Modable.mod(original.EventID, mod.EventID);
        EventGroup = Modable.mod(original.EventGroup, mod.EventGroup);
        EventStage = Modable.mod(original.EventStage, mod.EventStage);

    }

    public void mod(CommandEvent modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandEvent modCommand = modable as CommandEvent;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
