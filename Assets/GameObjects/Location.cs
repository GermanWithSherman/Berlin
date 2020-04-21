using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class Location : IModable
{
    public string id;

    public string Default = "main";

    //private Dictionary<string, SubLocation> subLocations = new Dictionary<string, SubLocation>();
    public ModableDictionary<SubLocation> subLocations = new ModableDictionary<SubLocation>();

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

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        subLocations.mod(((Location)modable).subLocations);
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
