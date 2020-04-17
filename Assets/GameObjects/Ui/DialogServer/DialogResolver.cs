using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogResolver
{
    private Dictionary<string, string> _targets;
    private Dictionary<string, Command> _onComplete = new Dictionary<string, Command>();

    public DialogResolver(IDictionary<string,string> targets)
    {
        foreach(KeyValuePair<string,string> kv in targets)
        {
            _targets.Add(kv.Key,kv.Value);
        }
    }

    public DialogResolver(JObject targets)
    {
        _targets = targets.ToObject<Dictionary<string, string>>();
    }

    public void resolve(DialogResult result)
    {
        GameData gameData = GameManager.Instance.GameData;

        foreach (KeyValuePair<string,string> kv in _targets)
        {
            string resultKey = kv.Key;
            string targetKey = kv.Value;

            gameData[targetKey] = result[resultKey];
        }

        foreach (Command command in _onComplete.Values)
        {
            command.execute();
        }
    }

    public void setOnComplete(JObject jObject)
    {
        if (jObject == null)
            return;
        try
        {
            _onComplete = jObject.ToObject<Dictionary<string, Command>>();
        }
        catch(Exception e)
        {
            _onComplete = new Dictionary<string, Command>();
            Debug.LogError($"Failed to set OnComplete with exception: {e.Message}");
        }
    }
}
