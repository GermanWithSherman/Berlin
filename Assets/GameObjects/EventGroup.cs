using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class EventGroup
{
    public string id;

    public string Default = "main";

    [JsonProperty]
    private Dictionary<string, EventStage> EventStages = new Dictionary<string, EventStage>();

    public EventStage this[string key]
    {
        get
        {
            if (String.IsNullOrEmpty(key))
                return EventStages[Default];

            if (EventStages.ContainsKey(key))
                return EventStages[key];

            Debug.LogWarning($"Requested sublocation {key} is not present in location {id}");
            return EventStages[Default];
        }
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (KeyValuePair<string, EventStage> kv in EventStages)
        {
            string eventId = kv.Key;
            EventStage eventStage = kv.Value;
            eventStage.id = id + "." + eventId;
        }

        if (!EventStages.ContainsKey(Default))
        {
            Debug.LogError($"EventGroup {id} deserialized with invalid default EventStage");
            EventStages[Default] = null;
        }
    }

}
