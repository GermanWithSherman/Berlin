using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubLocation : IModable
{
    [JsonIgnore]
    public string id;

    public LocationConnections LocationConnections = new LocationConnections();

    public CText Label = new CText();
    public CText Text = new CText();

    [JsonIgnore]
    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }
    public Conditional<string> TexturePath;// = new Conditional<string>();

    [JsonIgnore]
    public Texture TexturePreview {
        get
        {
            if(TexturePreviewPath != null)
                return GameManager.Instance.TextureCache[TexturePreviewPath];
            return Texture;
        }
    }
    public Conditional<string> TexturePreviewPath;

    public Dictionary<string, Option> Options = new Dictionary<string, Option>();

    public Schedules OpeningTimes = new Schedules();

    public CommandsCollection onShow = new CommandsCollection();

    public string Inherit;
    public bool inheritanceResolved = false;

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

    internal void mod(SubLocation modSublocation)
    {
        LocationConnections = Modable.mod(LocationConnections,modSublocation.LocationConnections);

        Text = Modable.mod(Text, modSublocation.Text);

        TexturePath = Modable.mod(TexturePath, modSublocation.TexturePath);
        TexturePreviewPath = Modable.mod(TexturePreviewPath, modSublocation.TexturePreviewPath);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((SubLocation)modable);
    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }
}
