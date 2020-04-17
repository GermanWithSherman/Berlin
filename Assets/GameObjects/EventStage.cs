using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStage
{
    [JsonIgnore]
    public string id;

    public CText Text = new CText();

    public CommandsCollection Commands = new CommandsCollection();

    public Dictionary<string, Option> Options = new Dictionary<string, Option>();

    public void execute()
    {
        GameManager gameManager = GameManager.Instance;

        Commands.execute();
    }

}
