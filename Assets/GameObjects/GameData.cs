using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameData: Data
{

    public DateTime savegameTime;

    public new dynamic this[string key]
    {
        get => get(key);
        set => set(key,value);
    }

    public CharacterData CharacterData = new CharacterData();
    public ShopData ShopData = new ShopData();
    public WorldData WorldData = new WorldData();

    public InterruptPersistentDataCollection Interrupts = new InterruptPersistentDataCollection();

    [JsonConverter(typeof(EventStageConverter))]
    public EventStage currentEventStage;

    [JsonIgnore]
    public SubLocation currentLocation;
    [JsonProperty]
    private string _currentLocationId;

    [JsonIgnore]
    public IEnumerable<NPC> NpcsPresent = new List<NPC>();

    [JsonExtensionData]
    private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' },2);

        if(keyParts.Length == 1)
        {
            switch (key)
            {
                case "example":
                    return "tatata";
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
                case "World":
                    return WorldData[keyParts[1]];
            }
        }

        if (_additionalData.ContainsKey(key))
        {
            switch (key[0])
            {
                case 'b':
                    return (bool)_additionalData[key];
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
                    return _additionalData[key].ToString();
            }
        }
        else
        {
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
                case "World":
                    WorldData[keyParts[1]] = value;
                    return;
            }
        }


        _additionalData[key] = value;


    }

    #region Serialization
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        GameManager gameManager = GameManager.Instance;

        currentLocation = gameManager.LocationCache.SubLocation(_currentLocationId);

        
    }

    /*internal void AfterLinkedMethod()
    {
        //We have to wait until GameManager set GameData to this
        List<NPC> npcsPresent = new List<NPC>();
        foreach (string id in _npcsPresentIds)
            //npcsPresent.Add(CharacterData[id]);
            npcsPresent.Add(GameManager.Instance.NPCsLibrary[id]);
        NpcsPresent = npcsPresent;
    }*/
    

    [OnSerializing]
    internal void OnSerializingMethod(StreamingContext context)
    {
        _currentLocationId = currentLocation.id;

        /*_npcsPresentIds = new List<string>();
        foreach (NPC npc in NpcsPresent)
            _npcsPresentIds.Add(npc.id);*/

        savegameTime = DateTime.UtcNow;
    }

    

    #endregion
}
