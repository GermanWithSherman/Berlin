using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class EventGroup
{
    public string id;

    //public List<EventStage> EventStages = new List<EventStage>();
    private List<EventStage> eventStages;
    public List<EventStage> EventStages
    {
        set => eventStages = value;
    }

    private Dictionary<string, EventStage> eventStagesDict = new Dictionary<string, EventStage>();

    public EventStage this[string key]
    {
        get => eventStagesDict[key];
    }

    [OnDeserialized]
    private void onDeserialized(StreamingContext context)
    {
        if (eventStages == null)
            return;

        foreach (EventStage eventStage in eventStages)
        {
            eventStagesDict[eventStage.id] = eventStage;
        }
    }
}
