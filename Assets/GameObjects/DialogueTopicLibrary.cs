using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueTopicLibrary
{
    private readonly Dictionary<string, DialogueTopic> dict = new Dictionary<string, DialogueTopic>();

    public DialogueTopic this[string key]
    {
        get => dict[key];
    }

    public DialogueTopicLibrary(string path)
    {
        loadFromFolder(path);
    }

    public List<DialogueTopic> getTopicsByNPC(NPC npc)
    {
        var result = new List<DialogueTopic>();
        foreach (DialogueTopic topic in dict.Values)
        {
            if (topic.NPCFilter.isValid(npc))
                result.Add(topic);
        }
        return result;
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
            IDictionary<string,DialogueTopic> topics = deserializationData.ToObject<Dictionary<string, DialogueTopic>>();

            foreach (KeyValuePair<string, DialogueTopic> kv in topics)
            {
                var topic = kv.Value;
                topic.ID = kv.Key;
                dict.Add(kv.Key,topic);
            }
        }

    }
}
