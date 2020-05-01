﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class LocationConnection : IModable
{
    [JsonIgnore]
    public SubLocation TargetLocation
    {
        get => GameManager.Instance.LocationCache.SubLocation(_targetLocationId);
    }


    private string _targetLocationId = "";
    public string targetLocationId
    {
        get => _targetLocationId;
        set => _targetLocationId = value;
    }

    public Conditional<string> TexturePath;
    public Conditional<bool> Visible;

    public Texture Texture {
        get{
            if (TexturePath != null)
                return GameManager.Instance.TextureCache[TexturePath];
            return TargetLocation.TexturePreview;
        }
    }

    public int? Duration;
    public string Label = "";

    public string Type = "Walk";
    public bool interruptible = true;

    public OutfitRequirement OutfitRequirement = new OutfitRequirement();


    public LocationConnection() { }

    public void execute()
    {
        if (!OutfitRequirement.isValid(GameManager.Instance.PC.currentOutfit))
        {
            ErrorMessage.Show(OutfitRequirement.Instruction);
            return;
        }
        GameManager.Instance.locationTransfer(this);
    }

    internal void linkIds(string locationId)
    {
        if (!String.IsNullOrEmpty(_targetLocationId) && _targetLocationId[0] == '.')
        {
            _targetLocationId = locationId + _targetLocationId;
        }
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        LocationConnection modLocationConnection = (LocationConnection)modable;

        Duration = modLocationConnection.Duration == null ? Duration : modLocationConnection.Duration;
    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }
}
