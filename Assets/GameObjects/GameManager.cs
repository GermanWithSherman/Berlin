﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const string PlayerVersion = "0.0.1";


    public static GameManager Instance { get; private set; }

    public Preferences Preferences;

    public Misc Misc;

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

    public ActivityLibrary ActivityLibrary;
    public DialogueTopicLibrary DialogueTopicLibrary;
    public FunctionsLibrary FunctionsLibrary;
    public ItemsLibrary ItemsLibrary;
    public LocationTypeLibrary LocationTypeLibrary;
    public NPCsLibrary NPCsLibrary;
    public ProceduresLibrary ProceduresLibrary;
    public TemplateLibrary TemplateLibrary;

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

            if (GameData.CurrentEventStage == null)
                return GameData.currentLocation.Options.Values;
            else
                return GameData.CurrentEventStage.Options.Values;
        }
    }

    public IEnumerable<LocationConnection> CurrentReachableLocations{get => GameData.currentLocation?.LocationConnections.VisibleLocationConnections;}

    public Template CurrentTemplate
    {
        get
        {
            if (GameData.currentLocation != null)
                return GameData.currentLocation.LocationType.Template;
            return TemplateLibrary[""];
        }
    }

    public string CurrentText
    {
        get
        {
            if (GameData.CurrentEventStage == null)
                return GameData.currentLocation.Text.Text(GameData);
            else
                return GameData.CurrentEventStage.Text.Text(GameData);
        }
    }

    public Texture CurrentTexture{
        get
        {
            if (GameData.CurrentEventStage == null || GameData.CurrentEventStage.Texture == null)
                return GameData.currentLocation?.Texture;
            else
                return GameData.CurrentEventStage.Texture;


        }
    }


    public CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-US");

    public GameObject SecondaryDisplayContent;
    private bool _secondaryDisplayActive;
    public bool SecondaryDisplayActive
    {
        get => _secondaryDisplayActive;
        set
        {
            _secondaryDisplayActive = value;
            if (_secondaryDisplayActive)
            {
                SecondaryDisplayContent.SetActive(true);
                Display.displays[1].Activate();
            }
            else
            {
                SecondaryDisplayContent.SetActive(false);
            }
        }
    }

    public UINPCsPresentContainer UINPCsPresentContainer;

    public OutfitWindow OutfitWindow;

    public UIDialogue UIDialogue;

    public UINotes UINotes;

    public UIPanelView UIServicesWindow;

    public UiShopWindow UiShopWindow;

    public Tooltip Tooltip;

    public StartMenu StartMenu;

    public GameObject LoadingScreen;

    private bool uiUpdateBlocked = false;
    private bool uiUpdatePending;
    public List<UIUpdateListener> updateListeners;

    public UISettings UISettings { get => GameData.UISettings; }

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
        if(Display.displays.Length > 1)
        {
            SecondaryDisplayActive = true;
            
        }


        ModsServer = new ModsServer(path("mods"), Preferences);

        loadStaticData();

        StartMenu.show();

        LoadingScreen.SetActive(false);

        UIDialogue.hide();
        UINotes.hide();
    }

    public void loadStaticData()
    {
        IEnumerable<string> modsPaths = ModsServer.ActivatedModsPaths;

        ActivityLibrary = new ActivityLibrary();

        DialogueTopicLibrary = new DialogueTopicLibrary(path("dialogue/topics"), pathsMods(modsPaths, "dialogue/topics"));
        Thread dialogueTopicLibraryLoadThread = DialogueTopicLibrary.loadThreaded();

        FunctionsLibrary = new FunctionsLibrary(path("functions"), pathsMods(modsPaths, "functions"));
        Thread functionsLibraryLoadThread = FunctionsLibrary.loadThreaded();

        ItemsLibrary = new ItemsLibrary(path("items"), pathsMods(modsPaths, "items"));
        Thread itemsLibraryLoadThread = ItemsLibrary.loadThreaded();

        InterruptServer = new InterruptServer(path("interrupts"), pathsMods(modsPaths, "interrupts"));
        Thread interruptServerLoadThread = InterruptServer.loadThreaded();

        LocationTypeLibrary = new LocationTypeLibrary(path("locationtypes"), pathsMods(modsPaths, "locationtypes"));
        Thread locationTypeLibraryLoadThread = LocationTypeLibrary.loadThreaded();

        ProceduresLibrary = new ProceduresLibrary(path("procedures"), pathsMods(modsPaths, "procedures"));
        Thread proceduresLibraryLoadThread = ProceduresLibrary.loadThreaded();

        TemplateLibrary = new TemplateLibrary(path("templates"), pathsMods(modsPaths, "templates"));
        Thread templateLibraryLoadThread = TemplateLibrary.loadThreaded();

        Thread rawNPCsLoadThread = NPCsLibrary.loadRawNpcsThreaded(path("npcs"), pathsMods(modsPaths, "npcs"));
        //NPCsLibrary.loadRawNpcsThreaded(path("npcs"), pathsMods(modsPaths, "npcs"));
        Misc = File2Object<Misc>(path("misc.json"));


        dialogueTopicLibraryLoadThread.Join();
        functionsLibraryLoadThread.Join();
        itemsLibraryLoadThread.Join();
        interruptServerLoadThread.Join();
        locationTypeLibraryLoadThread.Join();
        proceduresLibraryLoadThread.Join();
        rawNPCsLoadThread.Join();
        templateLibraryLoadThread.Join();
    }

    void Update()
    {
        if (uiUpdatePending && !uiUpdateBlocked)
            _uiUpdate();
    }


    public void console(string command)
    {
        Debug.Log(command);
    }

    public void dialogueContinue(string stageID)
    {
        UIDialogue.stageShow(stageID);
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
        GameData.CurrentEventStage = null;
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
        GameData.CurrentEventStage = eventStage;
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

    public static T File2Object<T>(string path)
    {
        return File2Data(path).ToObject<T>();
    }

    public void gameLoad()
    {
        gameLoad(QuicksavePath);
    }

    public void gameLoad(string path)
    {
        try
        {
            uiUpdateBlocked = true;

            SaveFile saveFile = File2Object<SaveFile>(path);
            Debug.Log($"Game loaded from {path}");

            List<string> problems = new List<string>();

            if(saveFile.PlayerVersion != PlayerVersion)
            {
                problems.Add($"PlayerVersion: {saveFile.PlayerVersion} > {PlayerVersion}");
            }

            foreach (ModInfo saveModInfo in saveFile.Mods)
            {
                ModInfo installedModInfo = ModsServer.ActivatedModInfo(saveModInfo.ID);
                if (String.IsNullOrEmpty(installedModInfo.ID))
                {
                    problems.Add($"Mod {saveModInfo.ID}: {saveModInfo.Version} > not installed");
                }else if (saveModInfo.Version != installedModInfo.Version)
                {
                    problems.Add($"Mod {saveModInfo.ID}: {saveModInfo.Version} > {installedModInfo.Version}");
                }
            }

            if (problems.Count > 0)
            {

                var dialogSettings = new YesNoDialogSettings();

                dialogSettings.Title = "Continue Loading?";
                dialogSettings.Text = "Error while loading savegame. Load anyway?\n" + String.Join("\n",problems);

                dialogSettings.onYes = delegate { gameDataLoad(saveFile.GameData); };
                dialogSettings.onNo = delegate { StartMenu.show(); };

                DialogServer.dialogShow(DialogServer.YesNoDialog, dialogSettings);
            }
            else
            {
                gameDataLoad(saveFile.GameData);
            }

            


        }
        catch(Exception e)
        {
            ErrorMessage.Show("Error: "+e.Message);
            Debug.LogError(e);
        }
        finally
        {
            uiUpdateBlocked = false;
        }
    }

    public void gameDataLoad(GameData gameData)
    {
        GameData = gameData;
        npcsPresentUpdate();
        uiUpdate();
        

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
        SaveFile saveFile = new SaveFile();
        saveFile.GameData = GameData;
        saveFile.Mods = ModsServer.ActivatedModsInfo;
        saveFile.PlayerVersion = PlayerVersion;
        Data2File(saveFile, path);
        Debug.Log($"Game save at {path}");

    }

    public void itemBuy(Item item, int price)
    {
        if(!PC.itemHas(item) && moneyPay(price))
            PC.itemAdd(item);
        uiUpdate();
    }

    public void locationGoto(string subLocationId, bool skipOnShow = false)
    {
        SubLocation subLocation = LocationCache.SubLocation(subLocationId);
        locationGoto(subLocation, skipOnShow);
    }

    public void locationGoto(SubLocation subLocation, bool skipOnShow = false)
    {
        

        GameData.CurrentEventStage = null;
        GameData.currentLocation = subLocation;

        npcsPresentUpdate();

        if(!skipOnShow)
            subLocation.onShowExecute(this);

        uiUpdate();
    }

    public bool npcIsPresent(string npcID)
    {
        return npcIsPresent(NPCsLibrary[npcID]);
    }

    public bool npcIsPresent(NPC npc)
    {
        foreach (NPC presentNPC in GameData.NpcsPresent)
        {
            if (presentNPC == npc)
                return true;
        }
        return false;
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

    internal void noteAdd(string noteID, string text)
    {
        GameData.Notes[noteID] = new Note(text, GameData.WorldData.DateTime);
    }

    internal void noteRemove(string noteID)
    {
        GameData.Notes.Remove(noteID);
    }

    public void notesHide()
    {
        UINotes.hide();
    }

    public void notesShow()
    {
        UINotes.showNotes(GameData.Notes.Values);
    }

    


    public void outfitWindowShow(OutfitRequirement outfitRequirement, CommandsCollection onClose)
    {
        OutfitWindow.show(outfitRequirement, onClose);
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

    public void ProcedureExecute(string procedureID, IEnumerable<dynamic> parameters)
    {
        ProceduresLibrary.procedureExecute(procedureID,GameData,parameters);
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

        Activity activity = ActivityLibrary[activityId];

        PC.timePass(seconds, activity);
    }

    public int timeSecondsTils(int targetTime,bool sameTimeAllowed=true)
    {
        DateTime now = GameData.WorldData.DateTime;

        int targetHour = targetTime / 10000;
        int targetMinute = (targetTime / 100) % 100;
        int targetSecond = targetTime % 100;

        int diff = (targetHour - now.Hour) * 3600 + (targetMinute - now.Minute) * 60 + (targetSecond - now.Second);

        if (diff < 0)
            diff += 86400;

        if(diff == 0 && !sameTimeAllowed)
            diff = 86400;

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
        try
        {
            Debug.Log($"DayNight: {Misc.dayNightState(GameData.WorldData.DateTime)}");


            uiUpdatePending = false;

            foreach (UIUpdateListener listener in updateListeners)
            {
                listener.uiUpdate(this);
            }
        }
        catch(Exception e)
        {
            ErrorMessage.Show($"Error performing UI-Update:\n{e}");
        }
    }

    
}
