using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueTopicLibrary : Library<DialogueTopic>
{
    
    public DialogueTopic this[string key]
    {
        get => _dict[key];
    }

    public DialogueTopicLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if(loadInstantly)
            load(path, modsPaths);
    }

    public List<DialogueTopic> getTopicsByNPC(NPC npc)
    {
        var result = new List<DialogueTopic>();
        foreach (DialogueTopic topic in _dict.Values)
        {
            if (topic.NPCFilter.isValid(npc))
                result.Add(topic);
        }
        return result;
    }

    protected override ModableDictionary<DialogueTopic> loadFromFolder(string path)
    {
        //_dict = new Dictionary<string, DialogueTopic>();
        var result = new ModableDictionary<DialogueTopic>();

        ModableDictionary<ModableDictionary<DialogueTopic>> dict = loadFromFolder<ModableDictionary<DialogueTopic>>(path);

       

        foreach(KeyValuePair<string, ModableDictionary<DialogueTopic>> kv in dict)
        {
            foreach (KeyValuePair<string, DialogueTopic> kv2 in kv.Value)
            {
                var topic = kv2.Value;
                topic.ID = kv2.Key;
                result.Add(kv2.Key, topic);
            }
        }

        return result;
    }

}
