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

    public ModableDictionary<Option> Options = new ModableDictionary<Option> ();

    [JsonIgnore]
    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }
    public Conditional<string> TexturePath;

    public void execute()
    {
        GameManager gameManager = GameManager.Instance;

        Commands.execute();
    }

}
