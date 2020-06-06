using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationConnections : ModableObjectSortedDictionary<LocationConnection>, IModable
{
    public LocationConnections() { }

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
        return copyDeep(this);
    }
}
