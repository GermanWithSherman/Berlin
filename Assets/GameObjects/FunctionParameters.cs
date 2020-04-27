using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionParameters : Data
{
    private Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

    public FunctionParameters() { }

    public FunctionParameters(string key, dynamic value)
    {
        data.Add(key, value);
    }

    public FunctionParameters(string key1, dynamic value1, string key2, dynamic value2)
    {
        data.Add(key1, value1);
        data.Add(key2, value2);
    }

    public void Add(string key, dynamic value)
    {
        data.Add(key,value);
    }

    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 1) {
            if (data.ContainsKey(key))
                return data[key];
            return GameManager.Instance.GameData[key];
        }
        return data[keyParts[0]][keyParts[1]];


    }

    protected override void set(string key, dynamic value)
    {
        throw new System.NotImplementedException();
    }
}
