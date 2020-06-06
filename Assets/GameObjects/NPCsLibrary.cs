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
        result.templateApply();

        //Get the persistant Data from GameData
        if(GameManager.Instance.GameData.CharacterData.NPCs.TryGetValue(key, out NPC persistantNPC))
        {
            result.mod(persistantNPC); //Overwrite all Data in RawNPC with Persistant Data
        }

        result.id = key;

        GameManager.Instance.GameData.CharacterData.NPCs[key] = result; //Make sure our changes are saved in the savegame

        return result;
    }



    public IEnumerable<NPC> npcsPresent(SubLocation subLocation, DateTime dateTime)
    {
        string subLocationID = subLocation.ID;
        List<NPC> result = new List<NPC>();

        IEnumerable<string> npcIds = Ids;

        foreach (string npcId in npcIds)
        {
            /*NPC npc;
            IEnumerable<Schedule> schedules;
            bool isRaw = false;
            if (GameData.CharacterData.has(npcId))
                npc = GameData.CharacterData[npcId];
            else
            {
                npc = NPCsRawLibrary[npcId];
                isRaw = true;
            }*/
            NPC npcRaw = rawNPCsLibrary[npcId];

            if(npcRaw.SchedulesDict.TryGetValue(subLocationID, out TimeFilters filters))
            {
                if(filters.isValid(dateTime))
                    result.Add(this[npcId]);
            }

            //IEnumerable<TimeFilters> schedules = npcRaw.Schedules;

            /*int time = dateTime.Minute + dateTime.Hour * 100;
            int day = (int)dateTime.DayOfWeek;

            foreach (TimeFilters schedule in schedules)
            {
                if (schedule.d.Contains(day) && time >= schedule.start && time <= schedule.end)
                {
                    if (schedule.l == subLocation.ID)
                    {
                        
                        result.Add(this[npcId]);
                    }
                    //else: the character is scheduled to be somewhere else
                    break;

                }
            }*/
        }

        return result;
    }
}
