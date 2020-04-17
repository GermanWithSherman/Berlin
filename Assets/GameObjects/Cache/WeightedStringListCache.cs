using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedStringListCache : Cache<WeightedStringList>
{
    protected override WeightedStringList create(string key)
    {
        string locationPath = System.IO.Path.Combine(GameManager.Instance.DataPath, "lists", key + ".txt");
        WeightedStringList weightedStringList = new WeightedStringList(locationPath);
        return weightedStringList;
    }
}
