using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationConnections : ModableDictionary<LocationConnection>
{
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
}
