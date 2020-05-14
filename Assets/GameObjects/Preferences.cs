using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class Preferences
{
    [JsonProperty]
    public string DataPath;
    [JsonProperty]
    public string SavePath;

    //public Dictionary<string, bool> Mods = new Dictionary<string, bool>();

    public delegate void OnUpdateMethod ();
    public OnUpdateMethod OnUpdate;

    [JsonProperty("ActivatedModIDs")]
    private List<string> _activatedModIDs = new List<string>();

    public List<string> ActivatedModIDs
    {
        get => _activatedModIDs;
        set
        {
            _activatedModIDs = value;
            OnUpdate();
        }
    }
    
}
