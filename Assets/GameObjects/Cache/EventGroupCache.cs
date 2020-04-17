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
}
