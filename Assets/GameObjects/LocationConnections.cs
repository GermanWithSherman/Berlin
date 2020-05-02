using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationConnections : ModableDictionary<LocationConnection>, IModable
{
    public LocationConnections() { }

    private LocationConnections(IModable modable)
    {
        if (modable is IDictionary<string, LocationConnection>)
            build((IDictionary<string, LocationConnection>)modable);
        else
            throw new NotImplementedException();
    }

    private LocationConnections(IDictionary<string, LocationConnection> dict)
    {
        build(dict);
    }

    private void build(IDictionary<string, LocationConnection> dict)
    {
        foreach (KeyValuePair<string, LocationConnection> kv in dict)
        {
            Add(kv.Key,kv.Value);
        }
    }


    [JsonIgnore]
    public List<LocationConnection> VisibleLocationConnections
    {
        get
        {
            List<LocationConnection> result = new List<LocationConnection>();

            foreach (LocationConnection locationConnection in Values)
            {
                if (locationConnection.Visible == null || locationConnection.Visible == true)
                    result.Add(locationConnection);
            }

            return result;
        }
    }

    public new IModable copyDeep()
    {
        return new LocationConnections(base.copyDeep());
    }
}
