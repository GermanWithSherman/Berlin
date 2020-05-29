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
    [JsonIgnore]
    public string ID;

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
                ErrorMessage.Show($"Requested sublocation {key} is not present in location {ID}");
                return Inheritable.Inherited(subLocations[Default]);
            }



        }

    }

    public IModable copyDeep()
    {
        var result = new Location();

        result.ID = Modable.copyDeep(ID);
        result.Default = Modable.copyDeep(Default);
        result.subLocations = Modable.copyDeep(subLocations);

        return result;
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

    /*[OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        
    }*/

    public void linkIds(string id)
    {
        this.ID = id;
        foreach (KeyValuePair<string, SubLocation> keyValuePair in subLocations)
        {
            SubLocation subLocation = keyValuePair.Value;
            subLocation.linkIds(ID, keyValuePair.Key);
        }

        if (!subLocations.ContainsKey(Default))
        {
            Debug.LogError($"Location {id} deserialized with invalid default sublocation");
            subLocations[Default] = null;
        }
    }
}
