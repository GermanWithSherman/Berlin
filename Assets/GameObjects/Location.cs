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
            SubLocation result;

            if (String.IsNullOrEmpty(key))
                result = subLocations[Default];

            else if (subLocations.ContainsKey(key))
                result = subLocations[key];
            else
            {
                Debug.LogWarning($"Requested sublocation {key} is not present in location {id}");
                result = subLocations[Default];
            }

            if (!result.inheritanceResolved)
            {
                if (!String.IsNullOrEmpty(result.Inherit))
                    result.mod(GameManager.Instance.LocationCache.SubLocation(result.Inherit));
                result.inheritanceResolved = true;
            }
            return result;

        }

    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
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
