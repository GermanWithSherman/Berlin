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
        //string locationPath = Path.Combine(GameManager.Instance.DataPath,"locations", key + ".json");

        List<string> locationPaths = new List<string>() { GameManager.Instance.DataPath };
        locationPaths.AddRange(GameManager.Instance.ModsServer.ActivatedModsPaths);

        Location result = null;

        foreach (string rawpath in locationPaths)
        {
            string path = Path.Combine(rawpath, "locations", key + ".json");
            Location location = loadLocation(path);
            if (location == null)
                continue;
            if (result == null)
            {
                result = location;
                continue;
            }
            result.mod(location);
        }

        //return loadLocation(locationPath);
        return result;
    }

    private Location loadLocation(string path)
    {
        if (!File.Exists(path))
            return null;

        JObject deserializationData = GameManager.File2Data(path);

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

            if (location == null)
                throw new Exception($"Location {keyparts[0]} not found");

            SubLocation subLocation = location[keyparts[1]];
            return subLocation;
        }

        return null;
    }

    
}
