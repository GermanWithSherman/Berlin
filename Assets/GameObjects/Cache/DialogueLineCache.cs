using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLineCache : Cache<DialogueStages>
{

    public DialogueStages this[DialogueTopic key]
    {
        get => this[key.ID];
    }


    public DialogueStage Stage(string stageId, DialogueTopic defaultTopic)
    {
        string[] keyparts = stageId.Split(new char[] { '.' }, 2);
        if (keyparts.Length != 2)
            throw new GameException("Invalid Stage ID");
        if (String.IsNullOrEmpty(keyparts[0]))
            return this[defaultTopic].stages[keyparts[1]];
        return this[keyparts[0]].stages[keyparts[1]];
    }
}
