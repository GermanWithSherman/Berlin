using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;




public class NPCsLibrary : Cache<NPC>
{
    public new NPC this[string key]
    {
        get
        {
            if (key == "PC")
                return GameManager.Instance.PC;
            return base[key];
        }
    }

    private class NPCsRawLibrary : Library<NPC>
    {
        public List<string> Ids
        {
            get => _dict.Keys.ToList(); //rawNPCDict.Keys.ToList();
        }

        public NPCsRawLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
        {
            this.path = path;
            this.modsPaths = modsPaths;
            if (loadInstantly)
                load(path, modsPaths);
        }
    }

    //private Dictionary<string, NPC> rawNPCDict = new Dictionary<string, NPC>();
    private NPCsRawLibrary rawNPCsLibrary;//

    public List<string> Ids
    {
        get => rawNPCsLibrary.Ids; //rawNPCDict.Keys.ToList();
    }



    public Thread loadRawNpcsThreaded(string path, IEnumerable<string> modsPaths)
    {
        
        rawNPCsLibrary = new NPCsRawLibrary(path, modsPaths);
        return rawNPCsLibrary.loadThreaded();
    }

    /*void Start()
    {
        loadFromFolder(GameManager.Instance.path(NPCsRawPath));
    }*/


    /*private void loadFromFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError($"Path {path} does not exist");
            return;
        }

        string[] filePaths = Directory.GetFiles(path);

        foreach (string filePath in filePaths)
        {

            JObject deserializationData = GameManager.File2Data(filePath);
            NPC npc = deserializationData.ToObject<NPC>();

            rawNPCDict.Add(Path.GetFileNameWithoutExtension(filePath),npc);
        }

    }*/

    protected override NPC create(string key)
    {
        NPC result = Modable.copyDeep(rawNPCsLibrary[key]); //we don't want realized NPCRaws to clutter rawNPCDict, that's what the cache is for
        //result.templateApply();
        result = NPC.templateApply(result);

        //Get the persistant Data from GameData
        if(GameManager.Instance.GameData.CharacterData.NPCs.TryGetValue(key, out NPC persistantNPC))
        {
            //result.mod(persistantNPC); //Overwrite all Data in RawNPC with Persistant Data
            result = Modable.mod(result,persistantNPC);
        }

        result.id = key;

        GameManager.Instance.GameData.CharacterData.NPCs[key] = result; //Make sure our changes are saved in the savegame

        return result;
    }

    internal dynamic getNPCorField(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 1)
            return this[key];

        NPC npc = this[keyParts[0]];
        return npc[keyParts[1]];
    }

    public string npcActivity(string npcID, string locationID, DateTime dateTime)
    {
        NPC npcRaw = rawNPCsLibrary[npcID];

        if (npcRaw.SchedulesDict.TryGetValue(locationID, out TimeFilters filters))
        {
            if(filters.tryGetValid(dateTime, out TimeFilter filter))
            {
                return filter.Activity;
            }
        }

        return "none";
    }

    public IList<NPC> npcsPresent(SubLocation subLocation, DateTime dateTime)
    {
        string subLocationID = subLocation.ID;
        List<NPC> result = new List<NPC>();

        IEnumerable<string> npcIds = Ids;

        foreach (string npcId in npcIds)
        {
            NPC npcRaw = rawNPCsLibrary[npcId];

            if(npcRaw.SchedulesDict.TryGetValue(subLocationID, out TimeFilters filters))
            {
                if(filters.isValid(dateTime))
                    result.Add(this[npcId]);
            }
        }

        return result;
    }
}
