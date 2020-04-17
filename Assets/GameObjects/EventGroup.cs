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

    [JsonProperty]
    private Dictionary<string, EventStage> EventStages = new Dictionary<string, EventStage>();

    public EventStage this[string key]
    {
        get => EventStages[key];
    }

}
