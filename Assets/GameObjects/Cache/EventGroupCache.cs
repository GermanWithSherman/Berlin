using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EventGroupCache : Cache<EventGroup>
{
    protected override EventGroup create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "events", key + ".json");

        JObject deserializationData = GameManager.File2Data(path);

        EventGroup eventGroupCache = deserializationData.ToObject<EventGroup>();

        return eventGroupCache;
    }

    public EventStage EventStage(string key)
    {
        string[] keyparts = key.Split('.');

        if (keyparts.Length == 2)
        {
            EventGroup eventGroup = this[keyparts[0]];
            EventStage eventStage = eventGroup[keyparts[1]];
            return eventStage;
        }

        return null;
    }

    public EventStage EventStage(string eventGroupId, string eventStageId)
    {
        EventGroup eventGroup = this[eventGroupId];
        EventStage eventStage = eventGroup[eventStageId];
        return eventStage;
    }
}
