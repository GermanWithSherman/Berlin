using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class Schedule : IModable
{
    /*public string l; // (Sub)LocationId
    public List<int> d; // Days of the week
    public int start; // Time of day as hhmm
    public int end;*/

    public string LocationID;
    public TimeFilters TimeFilters;

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new NotImplementedException();
    }
}
