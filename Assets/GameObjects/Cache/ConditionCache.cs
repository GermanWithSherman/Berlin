using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionCache : Cache<Condition>
{
    protected override Condition create(string key)
    {
        return new Condition(key);
    }

    protected override Condition getInvalidKeyEntry(string key)
    {
        return new Condition("");
    }
}
