using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonConverter(typeof(CommandConverter))]
public abstract class Command
{
    public static bool breakActive = false;
    public static bool pauseActive = false;
    public static CommandsCollection pausedCommands = new CommandsCollection();

    public enum Type { None, Debug, Break, Pause, Continue, Flush, Consume, Sleep, Dialog, Event, EventEnd, GotoLocation, Interrupt, Outfit, Services, Set, Shop, TimePass, ItemAdd }

    [JsonConverter(typeof(StringEnumConverter))]
    public Type type = Type.None;


    [JsonExtensionData]
    private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

    public void execute()
    {
        execute(GameManager.Instance.GameData);
    }
    public abstract void execute(Data data);

}
