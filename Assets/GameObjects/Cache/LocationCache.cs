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

        result.linkIds(key);

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
            return SubLocation(keyparts[0],keyparts[1]);
        }

        return null;
    }

    public SubLocation SubLocation(string locationId, string subLocationId)
    {
        Location location;
        location = this[locationId];

        if (location == null)
            throw new Exception($"Location {locationId} not found");

        SubLocation subLocation = location[subLocationId];
        return subLocation;
    }



}
