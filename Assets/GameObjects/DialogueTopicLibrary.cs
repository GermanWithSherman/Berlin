using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueTopicLibrary : Library<DialogueTopic>
{

    private List<DialogueTopic> _greetings;

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

    public DialogueTopic getGreetingTopicByNPC(NPC npc)
    {
        foreach (DialogueTopic topic in _greetings)
        {
            if (topic.NPCFilter.isValid(npc) && topic.Condition.evaluate(GameManager.Instance.GameData))
                return topic;
        }
        return null;
    }

    public List<DialogueTopic> getTopicsByNPC(NPC npc)
    {
        var result = new List<DialogueTopic>();
        foreach (DialogueTopic topic in _dict.Values)
        {
            if (!topic.IsGreeting && !topic.IsEventExclusive && topic.NPCFilter.isValid(npc) && topic.Condition.evaluate(GameManager.Instance.GameData))
                result.Add(topic);
        }
        return result;
    }

    protected override void load(string path, IEnumerable<string> modPaths)
    {
        base.load(path,modPaths);
        _greetings = new List<DialogueTopic>();
        foreach (DialogueTopic topic in _dict.Values)
        {
            if (topic.IsGreeting && !topic.IsEventExclusive)
                _greetings.Add(topic);
        }
        Prioritizable.Sort(_greetings);
    }

    protected override ModableDictionary<DialogueTopic> loadFromFolder(string path)
    {
        var result = new ModableDictionary<DialogueTopic>();

        ModableDictionary<DialogueTopicsFile> dict = loadFromFolder<DialogueTopicsFile>(path);

        foreach (DialogueTopicsFile dialogueTopicsFile in dict.Values)
        {
            foreach (KeyValuePair<string, DialogueTopic> kv2 in dialogueTopicsFile.topics)
            {
                var topic = kv2.Value;
                topic.ID = kv2.Key;
                result.Add(kv2.Key, topic);
            }
        }

        return result;
    }

}
