using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionParameters : Data
{
    private Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

    public FunctionParameters() { }

    public FunctionParameters(string rawParameters) : this(GameManager.Instance.GameData,rawParameters.Split(';')) {

    }

    public FunctionParameters(string key, dynamic value)
    {
        data.Add(key, value);
    }

    public FunctionParameters(string key1, dynamic value1, string key2, dynamic value2)
    {
        data.Add(key1, value1);
        data.Add(key2, value2);
    }

    public FunctionParameters(Data data,IEnumerable<string> keys)
    {
        if (keys == null)
            return;

        foreach (string key in keys)
        {
            string[] keyparts = key.Split(':');
            if(keyparts.Length == 1)
                this.data.Add(key, data[key]);
            else
                this.data.Add(keyparts[0], data[keyparts[1]]);
        }
    }

    public FunctionParameters(Data data, IEnumerable<dynamic> values)
    {
        if (values == null)
            return;

        int i = 1;

        foreach (dynamic value in values)
        {
            string parameterName = (i++).ToString();
            string s = value as string;
            if(s != null)
            {
                Add(parameterName,data[s]);
            }
            else
            {
                Add(parameterName, value);
            }
        }
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
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length < 2)
        {
            Debug.LogError($"Can't set {key} in FunctionParameters");
            return;
        }

        if (data.TryGetValue(keyParts[0], out dynamic entry))
        {
            Data entryData = entry as Data;
            if (entryData != null)
                entryData[keyParts[1]] = value;
            else
                Debug.LogError($"Key {key} is not Data in FunctionParameters");
        }
    }
}
