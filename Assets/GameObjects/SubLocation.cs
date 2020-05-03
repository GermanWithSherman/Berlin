﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SubLocation : IModable, IInheritable
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

    public ModableDictionary<Option> Options = new ModableDictionary<Option> ();

    public Schedules OpeningTimes = new Schedules();

    public CommandsCollection onShow = new CommandsCollection();

    public string Inherit;

    [JsonIgnore]
    public SubLocation Parent
    {
        get
        {
            if (String.IsNullOrEmpty(Inherit))
                return null;
            return GameManager.Instance.LocationCache.SubLocation(Inherit);
        }
    }
    [JsonIgnore]
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

    public void inherit(SubLocation parent)
    {
        SubLocation parentCopy = Modable.copyDeep(parent);

        mod(parentCopy, this);
        
    }

    public void inherit()
    {
        SubLocation parent = Parent;
        if(parent != null)
            inherit(parent);

        inheritanceResolved = true;
    }

    public bool isInheritanceResolved() => inheritanceResolved;

    private void mod(SubLocation original, SubLocation mod)
    {
        Label = Modable.mod(original.Label, mod.Label);

        LocationConnections = Modable.mod(original.LocationConnections, mod.LocationConnections);

        Options = Modable.mod(original.Options, mod.Options);

        Text = Modable.mod(original.Text, mod.Text);

        TexturePath = Modable.mod(original.TexturePath, mod.TexturePath);
        TexturePreviewPath = Modable.mod(original.TexturePreviewPath, mod.TexturePreviewPath);
    }

    public void mod(SubLocation modSublocation)
    {
        if (modSublocation == null) return;
        mod(this, modSublocation);
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
        var result = new SubLocation();
        result.Label = Modable.copyDeep(Label);
        result.LocationConnections = Modable.copyDeep(LocationConnections);
        result.Options = Modable.copyDeep(Options);
        result.Text = Modable.copyDeep(Text);
        result.TexturePath = Modable.copyDeep(TexturePath);
        result.TexturePreviewPath = Modable.copyDeep(TexturePreviewPath);
        return result;
    }

    
}
