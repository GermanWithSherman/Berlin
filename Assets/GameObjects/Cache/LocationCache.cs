using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocationCache : Cache<Location>
{

    protected override Location create(string key)
    {
        string locationPath = Path.Combine(GameManager.Instance.DataPath,"locations", key + ".json");

        JObject deserializationData = GameManager.File2Data(locationPath);

        Location location = deserializationData.ToObject<Location>();

        return location;
    }

    public SubLocation SubLocation(string key)
    {
        string[] keyparts = key.Split('.');

        if(keyparts.Length == 2)
        {
            Location location;
            location = this[keyparts[0]];
            SubLocation subLocation = location[keyparts[1]];
            return subLocation;
        }

        return null;
    }

    
}
