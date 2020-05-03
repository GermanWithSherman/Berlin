using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine : IModable
{
    public string TopicID;

    public int Priority = 0;

    public string Condition;

    public ModableDictionary<Option> Options = new ModableDictionary<Option>();

    public CText Text;

    public IModable copyDeep()
    {
        var result = new DialogueLine();

        result.TopicID = Modable.copyDeep(TopicID);
        result.Priority = Modable.copyDeep(Priority);
        result.Options = Modable.copyDeep(Options);
        result.Text = Modable.copyDeep(Text);

        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}

public class DialogueTopic : IModable
{
    [JsonIgnore]
    public string ID;

    public NPCFilter NPCFilter = new NPCFilter();

    public int Priority = 0;

    public CText Title;

    public IModable copyDeep()
    {
        var result = new DialogueTopic();

        result.NPCFilter = Modable.copyDeep(NPCFilter);
        result.Title = Modable.copyDeep(Title);

        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
