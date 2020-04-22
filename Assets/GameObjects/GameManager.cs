using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameData GameData = new GameData();

    public PC PC
    {
        get => GameData.CharacterData.PC;
    }

    public ConditionCache ConditionCache;
    public EventGroupCache EventGroupCache;
    public LocationCache LocationCache;
    public NPCTemplateCache NPCTemplateCache;
    public ServicesCache ServicesCache;
    public ServicepointCache ServicepointCache;
    public ShopTypeCache ShopTypeCache;
    public TextureCache TextureCache;
    public WeightedStringListCache WeightedStringListCache;

    public FunctionsLibrary FunctionsLibrary;
    public ItemsLibrary ItemsLibrary;
    public NPCsRawLibrary NPCsRawLibrary;

    public DialogServer DialogServer;
    public InterruptServer InterruptServer;
    public ModsServer ModsServer;

    public string DataPath;

    public string QuicksavePath;

    public TextMeshProUGUI TextBoxMain;
    public string TextMain
    {
        get => TextBoxMain.text;
        set => TextBoxMain.text = value;
    }

    public UIOptionsContainer OptionsContainer;

    public StatusBarHunger StatusBarHunger;
    public StatusBarMoney StatusBarMoney;
    public Text StatusBarDateTime;


    public RawImage ImageMain;
    public Texture TextureMain
    {
        get => ImageMain.texture;
        set => ImageMain.texture = value;
    }

    public UINPCsPresentContainer UINPCsPresentContainer;

    public UIRLocations UIRLocations;
    public IEnumerable<LocationConnection> LocationConnections
    {
        set => UIRLocations.setRLs(value);
    }

    public OutfitWindow OutfitWindow;

    public UIPanelView UIServicesWindow;

    public UiShopWindow UiShopWindow;

    public Tooltip Tooltip;

    public StartMenu StartMenu;

    private bool uiUpdatePending;


    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.LogError("Error: multiple " + this + " in scene!"); }
    }

    void Start()
    {
        FunctionsLibrary = new FunctionsLibrary(path("functions"));
        ItemsLibrary = new ItemsLibrary(path("items"));
        NPCsRawLibrary = new NPCsRawLibrary(path("npcs"));

        InterruptServer = new InterruptServer(path("interrupts"));
        ModsServer = new ModsServer(path("mods"));

        StartMenu.show();



    }

    void Update()
    {
        if (uiUpdatePending)
            _uiUpdate();
    }


    public void console(string command)
    {
        Debug.Log(command);
    }

    public void eventEnd()
    {
        GameData.currentEventStage = null;
        uiUpdate();
    }

    public void eventExecute(string eventId)
    {
        EventStage eventStage = EventGroupCache.EventStage(eventId);
        eventExecute(eventStage);
    }

    public void eventExecute(string eventGroupId, string eventStageId)
    {
        EventStage eventStage = EventGroupCache.EventStage(eventGroupId, eventStageId);
        eventExecute(eventStage);
    }

    public void eventExecute(EventStage eventStage)
    {
        GameData.currentEventStage = eventStage;
        eventStage?.execute();
        uiUpdate();
    }

    public static JObject File2Data(string path)
    {
        string jsonString = System.IO.File.ReadAllText(path);
        var data = JObject.Parse(jsonString);
        return data;
    }

    public void gameLoad()
    {
        gameLoad(QuicksavePath);
    }

    public void gameLoad(string path)
    {
        JObject deserializationData = GameManager.File2Data(path);
        GameData = deserializationData.ToObject<GameData>();
        npcsPresentUpdate();
        uiUpdate();
        Debug.Log($"Game loaded from {path}");

        OutfitWindow.setCharacter(PC);
    }

    public void gameNew()
    {
        eventExecute("start", "main");

        PC.age = 18;

        PC.outfits.Add("DEFAULT", new Outfit(new Item[] { ItemsLibrary["dress_2"] }));
        PC.currentOutfitId = "DEFAULT";

        OutfitWindow.setCharacter(PC);
    }

    public void gameSave()
    {
        gameSave(QuicksavePath);
    }

    public void gameSave(string path)
    {
        string json = JsonConvert.SerializeObject(GameData, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        System.IO.File.WriteAllText(path, json);
        Debug.Log($"Game save at {path}");

    }

    public void itemBuy(Item item, int price)
    {
        if(!PC.itemHas(item) && moneyPay(price))
            PC.itemAdd(item);
        uiUpdate();
    }

    public void locationGoto(string subLocationId)
    {
        //Location location = LocationCache[locationId];
        SubLocation subLocation = LocationCache.SubLocation(subLocationId);
        locationGoto(subLocation);
    }

    public void locationGoto(SubLocation subLocation)
    {
        subLocation.execute(this);

        GameData.currentEventStage = null;
        GameData.currentLocation = subLocation;

        npcsPresentUpdate();

        uiUpdate();
    }

    private void npcsPresentUpdate()
    {
        IEnumerable<NPC> npcs = npcsPresent(GameData.currentLocation, GameData.WorldData.DateTime);
        foreach (NPC npc in npcs)
            Debug.Log(FunctionsLibrary.npcName(npc));

        GameData.NpcsPresent = npcs;
        UINPCsPresentContainer.setNPCs(GameData.NpcsPresent);
    }

    public void locationTransfer(LocationConnection locationConnection)
    {

        CommandsCollection transferCommands = new CommandsCollection(locationConnection);
        transferCommands.execute();
    }

    public bool moneyPay(int amount)
    {
        GameData.CharacterData.PC.moneyCash -= amount;

        return true;
    }

    public IEnumerable<NPC> npcsPresent(SubLocation subLocation, DateTime dateTime)
    {
        List<NPC> result = new List<NPC>();

        IEnumerable<string> npcIds = NPCsRawLibrary.Ids;

        foreach(string npcId in npcIds)
        {
            NPC npc;
            IEnumerable<Schedule> schedules;
            bool isRaw = false;
            if (GameData.CharacterData.has(npcId))
                npc = GameData.CharacterData[npcId];
            else
            {
                npc = NPCsRawLibrary[npcId];
                isRaw = true;
            }
            schedules = npc.Schedules;

            int time = dateTime.Minute + dateTime.Hour * 100;
            int day = (int)dateTime.DayOfWeek;

            foreach(Schedule schedule in schedules)
            {
                if(schedule.d.Contains(day) && time >= schedule.start && time <= schedule.end)
                {
                    if (schedule.l == subLocation.id)
                    {
                        if (isRaw)
                        {
                            npc = npcGenerate(npcId);
                        }
                        result.Add(npc);
                    }
                    //else: the character is scheduled to be somewhere else
                    break;
                    
                }
            }
        }

        return result;
    }

    private NPC npcGenerate(string id)
    {
        NPCRaw npcRaw = NPCsRawLibrary[id];
        NPC npcGenerated = null;

        npcGenerated = npcRaw.generate();
        npcGenerated.id = id;

        GameData.CharacterData.NPCs.Add(id, npcGenerated);

        return npcGenerated;
    }

    public void optionsSet(IEnumerable<Option> options)
    {
        OptionsContainer.optionsSet(options);
    }

    public void outfitWindowShow()
    {
        OutfitWindow.show();
    }

    public string path(string p)
    {
        return Path.Combine(DataPath,p);
    }

     

    public void shopShow(string shopId)
    {
        Shop shop = GameData.ShopData[shopId];
        shopShow(shop);
    }

    public void shopShow(Shop shop)
    {
        UiShopWindow.show(shop);
    }

    public void servicepointShow(string id)
    {
        UIServicesWindow.setCategories(ServicepointCache[id].ServiceCategories);
        UIServicesWindow.show();
    }

    public void timeAdd(int duration)
    {
        GameData.WorldData.DateTime += new TimeSpan(0,0,duration);
    }

    public int timeAgeYears(DateTime dateTime)
    {
        DateTime now = GameData.WorldData.DateTime;

        int age = now.Year - dateTime.Year;

        if (dateTime.Month > now.Month || (dateTime.Month == now.Month && dateTime.Day > now.Day))
            age--;
        return age;
    }

    public void timePass(int seconds)
    {
        timeAdd(seconds);
        PC.timePass(seconds, new Activity());
    }

    public DateTime timeWithAge(int age)
    {
        DateTime todayMN = GameData.WorldData.DateTime.Date;

        DateTime ageYearsAgo = new DateTime(todayMN.Year - age, todayMN.Month, todayMN.Day);
        DateTime ageP1YearsAgo = new DateTime(todayMN.Year - age - 1, todayMN.Month, todayMN.Day);
        ageP1YearsAgo += new TimeSpan(1, 0, 0, 0);

        int days = (ageYearsAgo - ageP1YearsAgo).Days;

        ageP1YearsAgo += new TimeSpan(UnityEngine.Random.Range(0,days),0,0,0);

        return ageP1YearsAgo;

    }

    public void uiUpdate()
    {
        uiUpdatePending = true;

    }

    private void _uiUpdate()
    {
        uiUpdatePending = false;

        StatusBarHunger.set(PC.statHunger);
        StatusBarMoney.set(PC.moneyCash);
        StatusBarDateTime.text = GameData.WorldData.DateTime.ToString();
        TextureMain = GameData.currentLocation.Texture;

        UIServicesWindow.update();

        //SubLocation Stuff
        if (GameData.currentEventStage == null)
        {
            TextureMain = GameData.currentLocation.Texture;

            TextMain = GameData.currentLocation.Text.Text(GameData);

            //LocationConnections = GameData.currentLocation.LocationConnections.Values;
            LocationConnections = GameData.currentLocation.LocationConnections.VisibleLocationConnections;

            optionsSet(GameData.currentLocation.Options.Values);
        }
        else
        {
            TextMain = GameData.currentEventStage.Text.Text(GameData);
            optionsSet(GameData.currentEventStage.Options.Values);
            LocationConnections = null;
        }
    }

}
