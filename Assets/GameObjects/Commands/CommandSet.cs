using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSet : Command
{
    public ModableDictionary<dynamic> Values = new ModableDictionary<dynamic>();

    public override void execute(Data data)
    {
        foreach(KeyValuePair<string,dynamic> kv in Values)
        {
            data[kv.Key] = kv.Value;
            Debug.Log($"SET {kv.Key} => {kv.Value}");
        }
    }
}
