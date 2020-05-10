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

    public ModableDictionary<SubLocation> subLocations = new ModableDictionary<SubLocation>();

    public SubLocation this[string key]
    {
        get
        {
            //SubLocation result;

            if (String.IsNullOrEmpty(key))
                return Inheritable.Inherited(subLocations[Default]);
            else if (subLocations.ContainsKey(key))
                return Inheritable.Inherited(subLocations[key]);
            else
            {
                ErrorMessage.Show($"Requested sublocation {key} is not present in location {id}");
                return Inheritable.Inherited(subLocations[Default]);
            }



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
