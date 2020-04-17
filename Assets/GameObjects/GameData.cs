using Newtonsoft.Json;
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
    [JsonProperty]
    private List<string> _npcsPresentIds;

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
                    return CharacterData[keyParts[1]];
                case "PC":
                    return CharacterData.PC[keyParts[1]];
                case "Shop":
                    return ShopData[keyParts[1]];
                case "World":
                    return WorldData[keyParts[1]];
            }
        }

         return "";
    }

    protected override void set(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 2)
        {
            switch (keyParts[0])
            {
                case "NPC":
                    CharacterData[keyParts[1]] = value;
                    break;
                case "PC":
                    CharacterData[key] = value;
                    break;
                case "Shop":
                    ShopData[keyParts[1]] = value;
                    break;
                case "World":
                    WorldData[keyParts[1]] = value;
                    break;
            }
        }
    }

    #region Serialization
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        GameManager gameManager = GameManager.Instance;

        currentLocation = gameManager.LocationCache.SubLocation(_currentLocationId);

        List<NPC> npcsPresent = new List<NPC>();
        foreach (string id in _npcsPresentIds)
            npcsPresent.Add(CharacterData[id]);
        NpcsPresent = npcsPresent;
    }
    

    [OnSerializing]
    internal void OnSerializingMethod(StreamingContext context)
    {
        _currentLocationId = currentLocation.id;

        _npcsPresentIds = new List<string>();
        foreach (NPC npc in NpcsPresent)
            _npcsPresentIds.Add(npc.id);

        savegameTime = DateTime.UtcNow;
    }

    #endregion
}
