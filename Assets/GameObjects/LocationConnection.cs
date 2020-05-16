using Newtonsoft.Json;
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
        get => GameManager.Instance.LocationCache.SubLocation(TargetLocationId);
    }


    /*private string _targetLocationId = "";
    public string targetLocationId
    {
        get => _targetLocationId;
        set => _targetLocationId = value;
    }*/

    public string TargetLocationId;

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
        if (!String.IsNullOrEmpty(TargetLocationId) && TargetLocationId[0] == '.')
        {
            TargetLocationId = locationId + TargetLocationId;
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
        var result = new LocationConnection();

        result.Duration = Modable.copyDeep(Duration);
        result.interruptible = Modable.copyDeep(interruptible);
        result.Label = Modable.copyDeep(Label);
        result.TexturePath = Modable.copyDeep(TexturePath);
        result.Visible = Modable.copyDeep(Visible);
        result.TargetLocationId = Modable.copyDeep(TargetLocationId);
        result.OutfitRequirement = Modable.copyDeep(OutfitRequirement);
        result.Type = Modable.copyDeep(Type);

        return result;
    }
}
