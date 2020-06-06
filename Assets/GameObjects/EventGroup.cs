using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class EventGroup : IModable
{
    [JsonIgnore]
    public string id;

    public string Default = "main";

    [JsonProperty]
    private ModableObjectHashDictionary<EventStage> EventStages = new ModableObjectHashDictionary<EventStage>();

    public EventStage this[string key]
    {
        get
        {
            EventStage result;

            if (String.IsNullOrEmpty(key))
            {
                result = EventStages[Default];
                result.StageID = Default;
                return result;
            }

            if (EventStages.ContainsKey(key))
            {
                result = EventStages[key];
                result.StageID = key;
                return result;
            }

            Debug.LogWarning($"Requested sublocation {key} is not present in location {id}");
            result = EventStages[Default];
            result.StageID = Default;
            return result;
        }
    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new NotImplementedException();
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        /*foreach (KeyValuePair<string, EventStage> kv in EventStages)
        {
            string eventId = kv.Key;
            EventStage eventStage = kv.Value;
            eventStage.id = id + "." + eventId;
        }*/

        if (!EventStages.ContainsKey(Default))
        {
            Debug.LogError($"EventGroup {id} deserialized with invalid default EventStage");
            EventStages[Default] = null;
        }
    }

}
