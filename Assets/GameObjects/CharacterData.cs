using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CharacterData : Data
{

    public PC PC = new PC();
    public Dictionary<string, NPC> NPCs = new Dictionary<string, NPC>();



    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);
        if (keyParts.Length == 2)
        {
            if (keyParts[0] == "PC")
                return PC[keyParts[1]];
            //return NPCs[keyParts[0]][keyParts[1]];
            return npc(keyParts[0])[keyParts[1]];
        }
        else if (keyParts.Length == 1)
        {
            if (key == "PC")
                return PC;
            return NPCs[key];
        }
        return null;
    }

    private NPC npc(string key)
    {
        GameManager gameManager = GameManager.Instance;


        if (!NPCs.ContainsKey(key))
        {
            NPCs[key] = gameManager.NPCTemplateCache[key].generate();
        }
        
        return NPCs[key];

    }

    protected override void set(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);
        if (keyParts.Length == 2)
        {
            if (keyParts[0] == "PC")
            {
                PC[keyParts[1]] = value;
                return;
            }
            NPCs[keyParts[0]][keyParts[1]] = value;
        }
    }

    public bool has(string key)
    {
        if (key == "PC")
            return true;
        return NPCs.ContainsKey(key);
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (KeyValuePair<string, NPC> keyValuePair in NPCs)
            keyValuePair.Value.id = keyValuePair.Key;
    }
}
