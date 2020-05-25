using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : IModable
{
    public int statHunger = -23;
    public int statHygiene = -6;
    public int statSleep = -11;

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new NotImplementedException();
    }
}
