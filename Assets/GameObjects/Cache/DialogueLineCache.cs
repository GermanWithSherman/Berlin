using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLineCache : Cache<DialogueStages>
{
    public string DialogueLinePath;

    public DialogueStages this[DialogueTopic key]
    {
        get => this[key.ID];
    }

    protected override DialogueStages create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, DialogueLinePath, key + ".json");

        try
        {
            JObject deserializationData = GameManager.File2Data(path);

            DialogueStages dialogueStages = deserializationData.ToObject<DialogueStages>();

            return dialogueStages;
        }
        catch (FileNotFoundException)
        {
            throw new GameException($"ShopType {key} does not exist");
        }
    }

    public DialogueStage Stage(string stageId, DialogueTopic defaultTopic)
    {
        string[] keyparts = stageId.Split(new char[] { '.' }, 2);
        if (keyparts.Length != 2)
            throw new GameException("Invalid Stage ID");
        if (String.IsNullOrEmpty(keyparts[0]))
            return this[defaultTopic][keyparts[1]];
        return this[keyparts[0]][keyparts[1]];
    }
}
