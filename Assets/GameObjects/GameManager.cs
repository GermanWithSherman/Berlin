using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Preferences Preferences;

    public GameData GameData = new GameData();

    public PC PC
    {
        get => GameData.CharacterData.PC;
    }

    public ConditionCache ConditionCache;
    public DialogueLineCache DialogueLineCache;
    public EventGroupCache EventGroupCache;
    public LocationCache LocationCache;
    public NPCTemplateCache NPCTemplateCache;
    public ServicesCache ServicesCache;
    public ServicepointCache ServicepointCache;
    public ShopTypeCache ShopTypeCache;
    public TextureCache TextureCache;
    public WeightedStringListCache WeightedStringListCache;

    public DialogueTopicLibrary DialogueTopicLibrary;
    public FunctionsLibrary FunctionsLibrary;
    public ItemsLibrary ItemsLibrary;
    public NPCsLibrary NPCsLibrary;

    public DialogServer DialogServer;
    public InterruptServer InterruptServer;
    public ModsServer ModsServer;

    public string DataPath { get => Preferences.DataPath; }

    public string QuicksavePath;

    public TextMeshProUGUI TextBoxMain;
    public string TextMain
    {
        get => TextBoxMain.text;
        set => TextBoxMain.text = value;
    }

    public IEnumerable<Option> CurrentOptions
    {
        get
        {

            if (GameData.currentEventStage == null)
                return GameData.currentLocation.Options.Values;
            else
                return GameData.currentEventStage.Options.Values;
        }
    }

    public IEnumerable<LocationConnection> CurrentReachableLocations{get => GameData.currentLocation?.LocationConnections.VisibleLocationConnections;}

    public string CurrentText
    {
        get
        {
            if (GameData.currentEventStage == null)
                return GameData.currentLocation.Text.Text(GameData);
            else
                return GameData.currentEventStage.Text.Text(GameData);
        }
    }

    public Texture CurrentTexture{ get => GameData.currentLocation?.Texture; }

    public UINPCsPresentContainer UINPCsPresentContainer;


    public OutfitWindow OutfitWindow;

    public UIDialogue UIDialogue;

    public UIPanelView UIServicesWindow;

    public UiShopWindow UiShopWindow;

    public Tooltip Tooltip;

    public StartMenu StartMenu;

    public GameObject LoadingScreen;

    private bool uiUpdatePending;
    public List<UIUpdateListener> updateListeners;

    private void Awake()
    {
        LoadingScreen.SetActive(true);

        if (Instance == null) { Instance = this; } else { Debug.LogError("Error: multiple " + this + " in scene!"); }

        _preferencesPath = Path.Combine(Application.dataPath, "preferences.json");
        preferencesLoad();
    }

    private string _preferencesPath;

    private void preferencesLoad() => preferencesLoad(_preferencesPath);

    private void preferencesLoad(string path)
    {
        JObject jObject = File2Data(path);
        Preferences = jObject.ToObject<Preferences>();

        Preferences.OnUpdate = delegate { preferencesSave(path);  };
    }

    public void preferencesSave() => preferencesSave(_preferencesPath);

    private void preferencesSave(string path)
    {
        Data2File(Preferences, path);
    }

    void Start()
    {
        ModsServer = new ModsServer(path("mods"), Preferences);

        List<string> modsPaths = ModsServer.ActivatedModsPaths;
        DialogueTopicLibrary = new DialogueTopicLibrary(path("dialogue/topics"), pathsMods(modsPaths, "dialogue/topics") );
        Thread dialogueTopicLibraryLoadThread = DialogueTopicLibrary.loadThreaded();

        FunctionsLibrary = new FunctionsLibrary(path("functions"), pathsMods(modsPaths, "functions"));
        Thread functionsLibraryLoadThread = FunctionsLibrary.loadThreaded();

        ItemsLibrary = new ItemsLibrary(path("items"), pathsMods(modsPaths, "items"));
        Thread itemsLibraryLoadThread = ItemsLibrary.loadThreaded();

        InterruptServer = new InterruptServer(path("interrupts"), pathsMods(modsPaths, "interrupts"));
        Thread interruptServerLoadThread = InterruptServer.loadThreaded();

        dialogueTopicLibraryLoadThread.Join();
        functionsLibraryLoadThread.Join();
        itemsLibraryLoadThread.Join();
        interruptServerLoadThread.Join();

        StartMenu.show();

        LoadingScreen.SetActive(false);
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

    public void dialogueShow(NPC npc)
    {
        UIDialogue.show(npc);
    }

    public void dialogueShow(NPC npc, DialogueTopic topic)
    {
        UIDialogue.show(npc, topic);
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

    public static void Data2File(object value, string path)
    {
        string json = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        File.WriteAllText(path, json);
    }

    public static JObject File2Data(string path)
    {
        string jsonString = File.ReadAllText(path);
        var data = JObject.Parse(jsonString);
        return data;
    }

    public void gameLoad()
    {
        gameLoad(QuicksavePath);
    }

    public void gameLoad(string path)
    {
        JObject deserializationData = File2Data(path);
        GameData = deserializationData.ToObject<GameData>();
        npcsPresentUpdate();
        uiUpdate();
        Debug.Log($"Game loaded from {path}");

        OutfitWindow.setCharacter(PC);
    }

    public void gameNew()
    {
        eventExecute("start", "main");

        //PC.age = 18;

        OutfitWindow.setCharacter(PC);
    }

    public void gameSave()
    {
        gameSave(QuicksavePath);
    }

    public void gameSave(string path)
    {
        Data2File(GameData,path);
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
        SubLocation subLocation = LocationCache.SubLocation(subLocationId);
        locationGoto(subLocation);
    }

    public void locationGoto(SubLocation subLocation)
    {
        

        GameData.currentEventStage = null;
        GameData.currentLocation = subLocation;

        npcsPresentUpdate();

        subLocation.execute(this);

        uiUpdate();
    }

    private void npcsPresentUpdate()
    {
        IEnumerable<NPC> npcs = NPCsLibrary.npcsPresent(GameData.currentLocation, GameData.WorldData.DateTime);
        /*foreach (NPC npc in npcs)
            Debug.Log(FunctionsLibrary.npcName(npc));*/

        GameData.NpcsPresent = npcs;
        UINPCsPresentContainer.setNPCs(GameData.NpcsPresent);
    }

    public void locationTransfer(LocationConnection locationConnection)
    {

        CommandsCollection transferCommands = CommandGotoLocation.GotoCommandsList(locationConnection);
        transferCommands.execute();
    }

    public bool moneyPay(int amount)
    {
        GameData.CharacterData.PC.moneyCash -= amount;

        return true;
    }


    public void outfitWindowShow(OutfitRequirement outfitRequirement)
    {
        OutfitWindow.show(outfitRequirement);
    }

    public string path(string p)
    {
        return Path.Combine(DataPath,p);
    }

    public IEnumerable<string> pathsMods(IEnumerable<string> modBasePaths, string target)
    {
        var result = new List<string>();

        foreach (string modBasePath in modBasePaths)
        {
            result.Add(Path.Combine(modBasePath, target));
        }

        return result;
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
        uiUpdate();
    }

    public int timeAgeYears(DateTime dateTime)
    {
        DateTime now = GameData.WorldData.DateTime;

        int age = now.Year - dateTime.Year;

        if (dateTime.Month > now.Month || (dateTime.Month == now.Month && dateTime.Day > now.Day))
            age--;
        return age;
    }

    public void timePass(int seconds, string activityId = "default")
    {
        timeAdd(seconds);

        Activity activity = new Activity();

        if(activityId == "sleep")
        {
            activity.statSleep *= -2;
            activity.statHunger /= 2;
        }

        PC.timePass(seconds, activity);
    }

    public int timeSecondsTils(int targetTime)
    {
        DateTime now = GameData.WorldData.DateTime;

        int targetHour = targetTime / 10000;
        int targetMinute = (targetTime / 100) % 100;
        int targetSecond = targetTime % 100;

        int diff = (targetHour - now.Hour) * 3600 + (targetMinute - now.Minute) * 60 + (targetSecond - now.Second);

        if (diff < 0)
            diff += 86400;

        return diff;
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

        foreach (UIUpdateListener listener in updateListeners)
        {
            listener.uiUpdate(this);
        }
    }

}
