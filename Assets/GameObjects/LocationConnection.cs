using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class LocationConnection
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

    public Texture Texture {
        get{
            if (TexturePath != null)
                return GameManager.Instance.TextureCache[TexturePath];
            return TargetLocation.Texture;
        }
    }

    public int Duration = 0;
    public string Label = "";

    public void execute()
    {
        GameManager.Instance.locationTransfer(this);
    }

    internal void linkIds(string locationId)
    {
        if (_targetLocationId[0] == '.')
        {
            _targetLocationId = locationId + _targetLocationId;
        }
    }

}
