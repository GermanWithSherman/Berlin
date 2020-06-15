using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataComposed : Data
{
    private IEnumerable<Data> _readData;
    private Data _writeData;


    public DataComposed(Data readData, Data writeData)
    {
        _readData = new List<Data>() { readData };
        _writeData = writeData;
    }

    public DataComposed(IEnumerable<Data> readData, Data writeData)
    {
        _readData = readData;
        _writeData = writeData;
    }

    public DataComposed(IDictionary<string, Data> readData)
    {
        List<Data> list = new List<Data>() { new DataCombined(readData) };
        list.Add(GameManager.Instance.GameData);
        _readData = list;
        _writeData = GameManager.Instance.GameData;
    }

    public DataComposed(IDictionary<string,Data> readData, Data writeData, bool includeGameData=true)
    {

        List<Data> list = new List<Data>() { new DataCombined(readData) };
        if (includeGameData)
            list.Add(GameManager.Instance.GameData);
        _readData = list;
        _writeData = writeData;
    }

    public override bool tryGetValue(string key, out dynamic result)
    {
        foreach (Data data in _readData)
        {
            if(data.tryGetValue(key, out result))
                return true;
        }
        result = null;
        return false;
    }

    protected override dynamic get(string key)
    {
        foreach (Data data in _readData)
        {
            if (data.tryGetValue(key, out dynamic result))
                return result;
        }
        return null;
    }

    protected override void set(string key, dynamic value)
    {
        _writeData[key] = value;
    }
}

public class DataCombined : Data
{

    private IDictionary<string,Data> _dict;

    public DataCombined(IDictionary<string, Data> dict)
    {
        _dict = dict;
    }

    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 2)
        {
            if (_dict.ContainsKey(keyParts[0]))
                return _dict[keyParts[0]][keyParts[1]];
        }
        else
        {
            if (_dict.ContainsKey(key))
                return _dict[key];
        }

        return null;
    }

    protected override void set(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 2)
        {
            if (_dict.ContainsKey(keyParts[0]))
                _dict[keyParts[0]][keyParts[1]] = value;
            else
                throw new System.Exception($"{keyParts[0]} does not exist in Set Key {key}");
        }
        else
        {
            throw new System.Exception($"Malformed Set Key {key}");
        }

    }
}
