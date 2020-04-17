using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubLocation
{
    [JsonIgnore]
    public string id;

    public Dictionary<string,LocationConnection> LocationConnections = new Dictionary<string, LocationConnection>();

    public CText Label = new CText();
    public CText Text = new CText();

    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }
    public Conditional<string> TexturePath;

    public Dictionary<string, Option> Options = new Dictionary<string, Option>();

    public Schedules OpeningTimes = new Schedules();

    public CommandsCollection onShow = new CommandsCollection();

    public void execute(GameManager gameManager)
    {

        onShow.execute();

    }

    public bool isOpen()
    {
        GameManager gameManager = GameManager.Instance;
        DateTime now = gameManager.GameData.WorldData.DateTime;
        return OpeningTimes.isScheduled(now);
    }

    internal void linkIds(string locationId, string subLocationId)
    {
        id = locationId + "." + subLocationId;

        foreach (LocationConnection locationConnection in LocationConnections.Values)
        {
            locationConnection.linkIds(locationId);
        }
    }

}
