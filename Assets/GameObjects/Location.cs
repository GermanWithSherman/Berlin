using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string id;

    public string Default = "main";

    [JsonProperty]
    private Dictionary<string, SubLocation> subLocations = new Dictionary<string, SubLocation>();

    public SubLocation this[string key]
    {
        get
        {
            if (String.IsNullOrEmpty(key))
                return subLocations[Default];

            if (subLocations.ContainsKey(key))
                return subLocations[key];

            Debug.LogWarning($"Requested sublocation {key} is not present in location {id}");
            return subLocations[Default];
        }

    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach(KeyValuePair<string,SubLocation> keyValuePair in subLocations)
        {
            SubLocation subLocation = keyValuePair.Value;
            subLocation.linkIds(id,keyValuePair.Key);
        }

        if (!subLocations.ContainsKey(Default))
        {
            Debug.LogError($"Location {id} deserialized with invalid default sublocation");
            subLocations[Default] = null;
        }
    }
}
