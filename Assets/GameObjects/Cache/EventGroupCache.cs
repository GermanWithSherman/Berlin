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
        EventStage result = null;

        if (keyparts.Length == 2)
        {
            EventGroup eventGroup = this[keyparts[0]];
            EventStage eventStage = eventGroup[keyparts[1]];
            result = eventStage;
            result.GroupID = keyparts[0];
            result.StageID = keyparts[1];
        }
        else if(keyparts.Length == 1)
        {
            Debug.LogWarning($"Malformed EventStage Key '{key}', assuming '{key}.'");
            EventGroup eventGroup = this[key];
            EventStage eventStage = eventGroup[""];
            
            result = eventStage;
            result.GroupID = key;
            result.StageID = "";
        }

        result = Modable.copyDeep(result);

        result.inherit();
        /*foreach(string InheritID in result.InheritIDs)
        {
            result = Modable.mod(EventStage(InheritID), result);
        }*/

        

        return result;
    }

    public EventStage EventStage(string eventGroupId, string eventStageId)
    {
        /*EventGroup eventGroup = this[eventGroupId];
        EventStage eventStage = eventGroup[eventStageId];*/
        return EventStage($"{eventGroupId}.{eventStageId}");
        //return eventStage;
    }
}
