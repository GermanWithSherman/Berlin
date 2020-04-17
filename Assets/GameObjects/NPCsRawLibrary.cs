using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class NPCsRawLibrary
{
    private Dictionary<string, NPCRaw> dict = new Dictionary<string, NPCRaw>();

    public List<string> Ids
    {
        get => dict.Keys.ToList();
    }

    public NPCRaw this[string key]
    {
        get => dict[key];
    }

    public NPCsRawLibrary(string path)
    {
        loadFromFolder(path);
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
            NPCRaw npc = deserializationData.ToObject<NPCRaw>();

            dict.Add(Path.GetFileNameWithoutExtension(filePath),npc);
        }

    }
}
