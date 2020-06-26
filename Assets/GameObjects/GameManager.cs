using Newtonsoft.Json;
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

    public GameData GameData;// = new GameData();

    public PC PC
    {
        get => GameData.CharacterData.PC;
    }

    #region Caches
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

    public void CacheLocationsReset()
    {
        LocationCache.Reset();
    }

    public void CachesReset()
    {
        CacheLocationsReset();
        DialogueLineCache.Reset();
        EventGroupCache.Reset();
        NPCTemplateCache.Reset();
        ServicesCache.Reset();
        ServicepointCache.Reset();
        ShopTypeCache.Reset();
        TextureCache.Reset();
        WeightedStringListCache.Reset();
    }

    #endregion
    #region Libraries
    public ActivityLibrary ActivityLibrary;
    public DialogueTopicLibrary DialogueTopicLibrary;
    public FunctionsLibrary FunctionsLibrary;
    public ItemsLibrary ItemsLibrary;
    public LocationTypeLibrary LocationTypeLibrary;
    public NPCsLibrary NPCsLibrary;
    public ProceduresLibrary ProceduresLibrary;
    public TemplateLibrary TemplateLibrary;
    #endregion
    #region Servers
    public DialogServer DialogServer;
    public InterruptServer InterruptServer;
    public ModsServer ModsServer;
    #endregion

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
                return GameData.CurrentLocation.Options.Values;
            else
                return GameData.CurrentEventStage.Options.Values;
        }
    }

    public IEnumerable<LocationConnection> CurrentReachableLocations
    {
        get
        {
            if(GameData.CurrentEventStage == null)
                return GameData.CurrentLocation?.LocationConnections.VisibleLocationConnections;
            return new LocationConnection[0];
        }
    }

    public Template CurrentTemplate
    {
        get
        {
            if (GameData.CurrentLocation != null)
                return GameData.CurrentLocation.LocationType.Template;
            return TemplateLibrary[""];
        }
    }

    public string CurrentText
    {
        get
        {
            if (GameData.CurrentEventStage == null)
                return GameData.CurrentLocation.Text.Text(GameData);
            else
                return GameData.CurrentEventStage.Text.Text(GameData);
        }
    }

    public Texture CurrentTexture{
        get
        {
            if (GameData.CurrentEventStage == null || GameData.CurrentEventStage.Texture == null)
                return GameData.CurrentLocation?.Texture;
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

    public CharacterScreen CharacterScreen;

    public EditWindow EditWindow;

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

       
        preferencesLoad();
    }


    private string _preferencesPath;

    private void preferencesLoad() => preferencesLoad(Application.dataPath);

    private void preferencesLoad(string path)
    {
        _preferencesPath = Path.Combine(path, "preferences.json");
        if (!File.Exists(_preferencesPath))
        {
            Preferences = new Preferences(path);
            preferencesSave(_preferencesPath);
        }
        else
        {
            JObject jObject = File2Data(_preferencesPath);
            Preferences = jObject.ToObject<Preferences>();
        }

        Preferences.OnUpdate = delegate { preferencesSave(_preferencesPath);  };
    }

    public void preferencesSave() => preferencesSave(_preferencesPath);

    private void preferencesSave(string path)
    {
        Data2File(Preferences, path);
    }

    void Start()
    {
        try
        {
            if (Display.displays.Length > 1)
            {
                SecondaryDisplayActive = true;

            }


            ModsServer = new ModsServer(path("mods"), Preferences);

            loadStaticData();

            StartMenu.show();

            LoadingScreen.SetActive(false);

            CharacterScreen.hide();
            EditWindow.hide();
            UIDialogue.hide();
            UINotes.hide();
        }
        catch(Exception e)
        {
            LoadingScreen.SetActive(false);
            ErrorMessage.Show(e.Message);
            Debug.LogError(e);
        }
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

    public void CharacterScreenShow()
    {
        CharacterScreen.show(PC);
    }

    public void console(string command)
    {
        Debug.Log(command);
    }

    public void dialogueContinue(string topicID)
    {

        UIDialogue.contin(topicID);
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

        if (GameData.CurrentEventStage != null)
            UINPCsPresentContainer.setNPCs(new NPC[0]);

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
        if (!File.Exists(path))
            return new JObject();
        try
        {
            
            string jsonString = File.ReadAllText(path);
            var data = JObject.Parse(jsonString);
            return data;
        }
        catch
        {
            return new JObject();
        }
        
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

        PC.id = "PC";
    }

    public void gameNew()
    {
        GameData = new GameData();

        PC pc = new PC();
        pc.id = "PC";
        GameData.CharacterData.PC = pc;

        GameData.NPCsActive["_PC"] = pc;

        eventExecute("start", "main");

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
        GameData.CurrentLocation = subLocation;

        npcsPresentUpdate();

        if(!skipOnShow)
            subLocation.onShowExecute(this);

        uiUpdate();
    }

    public string npcActivity(string npcID)
    {
        return npcActivity(npcID,GameData.CurrentLocation.ID);
    }

    public string npcActivity(string npcID, string locationID)
    {
        return NPCsLibrary.npcActivity(npcID, locationID, GameData.WorldData.DateTime);
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



    public bool npcIsPresent(string npcID, string locationID)
    {
        NPC npc = NPCsLibrary[npcID];
        SubLocation subLocation = LocationCache.SubLocation(locationID);
        return npcIsPresent(npc,subLocation);
    }

    public bool npcIsPresent(NPC npc, SubLocation subLocation)
    {
        IList<NPC> npcsPresent = NPCsLibrary.npcsPresent(subLocation, GameData.WorldData.DateTime);

        if (npcsPresent.Contains(npc))
            return true;
        return false;
    }

    private void npcsPresentUpdate()
    {
        IEnumerable<NPC> npcs = NPCsLibrary.npcsPresent(GameData.CurrentLocation, GameData.WorldData.DateTime);

        GameData.NpcsPresent = npcs;

        if (GameData.CurrentEventStage == null)
            UINPCsPresentContainer.setNPCs(GameData.NpcsPresent);
        else
            UINPCsPresentContainer.setNPCs(new NPC[0]);
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

    public IEnumerable<string> pathsMods(string target)
    {
        return pathsMods(ModsServer.ActivatedModsPaths, target);
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

    public void ProcedureExecute(string procedureID, IEnumerable<dynamic> parameters,Data data)
    {
        ProceduresLibrary.procedureExecute(procedureID, data, parameters);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Refresh()
    {
        CachesReset();
        loadStaticData();

        npcsPresentUpdate();
        uiUpdate();
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
            DataCache.Reset();

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
