using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameData: Data
{

    

    /*public new dynamic this[string key]
    {
        get => get(key);
        set => set(key,value);
    }*/

    public UISettings UISettings = new UISettings();

    public CharacterData CharacterData = new CharacterData();
    public ShopData ShopData = new ShopData();
    public WorldData WorldData = new WorldData();

    public InterruptPersistentDataCollection Interrupts = new InterruptPersistentDataCollection();

    public Dictionary<string, Note> Notes = new Dictionary<string, Note>();

    [JsonConverter(typeof(EventStageConverter))]
    public EventStage CurrentEventStage;

    [JsonIgnore]
    public SubLocation currentLocation;
    [JsonProperty("CurrentLocationId")]
    private string _currentLocationId
    {
        get => currentLocation?.ID;
        set { currentLocation = GameManager.Instance.LocationCache.SubLocation(value); }
    }

    [JsonIgnore]
    public IEnumerable<NPC> NpcsPresent = new List<NPC>();

    [JsonExtensionData]
    private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' });

        if(keyParts.Length == 1)
        {
            switch (key)
            {
                case "PC":
                    return CharacterData.PC;
                case "UI":
                    return UISettings;
                case "World":
                    return WorldData;
            }
        }
        else if (keyParts.Length == 2)
        {
            switch (keyParts[0])
            {
                case "NPC":
                    //return CharacterData[keyParts[1]];
                    //We need to acquire NPC-Data from the NPCsLibrary because the Data in Savegames is incomplete (it lacks Schedules etc.)
                    return GameManager.Instance.NPCsLibrary[keyParts[1]];
                case "PC":
                    //Other than NPCs PC-Data is not split
                    return CharacterData.PC[keyParts[1]];
                case "Shop":
                    return ShopData[keyParts[1]];
                case "UI":
                    return UISettings[keyParts[1]];
                case "World":
                    return WorldData[keyParts[1]];
            }
        }
        else if (keyParts.Length == 3)
        {
            switch (keyParts[0])
            {
                case "NPC":
                    return GameManager.Instance.NPCsLibrary[keyParts[1]][keyParts[2]];
            }
        }

        if (_additionalData.ContainsKey(key))
        {
            try
            {
                switch (key[0])
                {
                    case 'b':
                        return (bool)_additionalData[key];
                    case 'c':
                        return (DateTime)_additionalData[key];
                    case 'i':
                        return (int)_additionalData[key];
                    case 'f':
                        return (float)_additionalData[key];
                    case 'd':
                        return (double)_additionalData[key];
                    case 'm':
                        return (decimal)_additionalData[key];
                    case 's':
                    default:
                        if (_additionalData[key] == null)
                            return "";
                        return _additionalData[key].ToString();
                }
            }
            catch
            {
                Debug.LogError("Failed to cast {key} in GameData.get()");
            }
        }
        
        Debug.LogWarning($"Requesting unknown value {key} from Gamedata. Returning default data.");
        switch (key[0])
        {
            case 'b':
                return false;
            case 'i':
                return 0;
            case 'f':
                return 0f;
            case 'd':
                return 0d;
            case 'm':
                return 0m;
            case 's':
            default:
                return "";
        }
        

    }


    protected override void set(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 2)
        {
            switch (keyParts[0])
            {
                case "NPC":
                    //Persistant Data has to be written to GameData, not to libraries!
                    CharacterData[keyParts[1]] = value;
                    return;
                case "PC":
                    CharacterData[key] = value;
                    return;
                case "Shop":
                    ShopData[keyParts[1]] = value;
                    return;
                case "UI":
                    UISettings[keyParts[1]] = value;
                    return;
                case "World":
                    WorldData[keyParts[1]] = value;
                    return;
            }
        }


        _additionalData[key] = value;


    }

    #region Serialization
    /*[OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        GameManager gameManager = GameManager.Instance;

        currentLocation = gameManager.LocationCache.SubLocation(_currentLocationId);

        
    }*/
    

    

    

    #endregion
}
