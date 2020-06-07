using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class EventGroup : IModable, IModableAutofields
{
    [JsonIgnore]
    public string id;

    public string Default = "main";

    public ModableObjectHashDictionary<EventStage> EventStages = new ModableObjectHashDictionary<EventStage>();

    public EventGroup()
    {
    }

    public EventStage this[string key]
    {
        get
        {
            try
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

                Debug.LogWarning($"Requested sublocation {key} is not present in event {id}");
                result = EventStages[Default];
                result.StageID = Default;
                return result;
            }
            catch
            {
                string json = JsonConvert.SerializeObject(EventStages, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                Debug.LogWarning(json);
                return default;
            }
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

    /*[OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {

        if (!EventStages.ContainsKey(Default))
        {
            Debug.LogError($"EventGroup {id} deserialized with invalid default EventStage");
            EventStages[Default] = null;
        }
    }*/

}
