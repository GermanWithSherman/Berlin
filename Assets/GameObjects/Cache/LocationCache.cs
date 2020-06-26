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
        var result = base.create(key);

        result?.linkIds(key);

        return result;

    }


    public SubLocation SubLocation(string key)
    {
        if (String.IsNullOrEmpty(key))
            return null;

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
