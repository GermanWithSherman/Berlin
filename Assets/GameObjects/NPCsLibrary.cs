using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NPCsLibrary : Cache<NPC>
{
    public string NPCsRawPath;

    private Dictionary<string, NPC> rawNPCDict = new Dictionary<string, NPC>();

    public List<string> Ids
    {
        get => rawNPCDict.Keys.ToList();
    }

    /*public NPCRaw this[string key]
    {
        get => dict[key];
    }*/

    /*public NPCsLibrary(string path)
    {
        loadFromFolder(path);
    }*/

    void Start()
    {
        loadFromFolder(GameManager.Instance.path(NPCsRawPath));
    }


    private void loadFromFolder(string path)
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

    }

    protected override NPC create(string key)
    {
        NPC result = Modable.copyDeep(rawNPCDict[key]); //we don't want realized NPCRaws to clutter rawNPCDict, that's what the cache is for
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
            NPC npcRaw = rawNPCDict[npcId];

            IEnumerable<Schedule> schedules = npcRaw.Schedules;

            int time = dateTime.Minute + dateTime.Hour * 100;
            int day = (int)dateTime.DayOfWeek;

            foreach (Schedule schedule in schedules)
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
            }
        }

        return result;
    }
}
