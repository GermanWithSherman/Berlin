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

    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 1)
            return data[key];
        return data[keyParts[0]][keyParts[1]];
    }

    protected override void set(string key, dynamic value)
    {
        throw new System.NotImplementedException();
    }
}
