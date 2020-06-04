using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine : IModable, IPrioritizable
{
    //public string TopicID;

    public bool TopicsVisible = true;
    public bool LeaveEnabled = true;

    public CommandsCollection onShow = new CommandsCollection();

    public int Priority = 0;

    [JsonProperty("Condition")]
    public string ConditionString;

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[ConditionString];
    }

    public ModableDictionary<DialogueOption> Options = new ModableDictionary<DialogueOption>();

    [JsonProperty("Text")]
    private CText _text;
    

    public IModable copyDeep()
    {
        var result = new DialogueLine();

        //result.TopicID = Modable.copyDeep(TopicID);
        result.Priority = Modable.copyDeep(Priority);
        result.Options = Modable.copyDeep(Options);
        result._text = Modable.copyDeep(_text);
        result.ConditionString = Modable.copyDeep(ConditionString);
        result.LeaveEnabled = Modable.copyDeep(LeaveEnabled);
        result.TopicsVisible = Modable.copyDeep(TopicsVisible);
        result.onShow = Modable.copyDeep(onShow);

        return result;
    }

    public int getPriority()
    {
        return Priority;
    }

    private void mod(DialogueLine original, DialogueLine mod)
    {
        Priority = Modable.mod(original.Priority, mod.Priority);
        Options = Modable.mod(original.Options, mod.Options);
        _text = Modable.mod(original._text, mod._text);
        ConditionString = Modable.mod(original.ConditionString, mod.ConditionString);
        LeaveEnabled = Modable.mod(original.LeaveEnabled, mod.LeaveEnabled);
        TopicsVisible = Modable.mod(original.TopicsVisible, mod.TopicsVisible);
        onShow = Modable.mod(original.onShow, mod.onShow);
    }

    public void mod(DialogueLine modSublocation)
    {
        if (modSublocation == null) return;
        mod(this, modSublocation);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((DialogueLine)modable);
    }

    public string Text(Data data)
    {
        return _text.Text(data);
    }
}

public class DialogueOption : Option, IModable
{
    public string TargetStage;
    public string Say;

    public new IModable copyDeep()
    {
        DialogueOption result = (DialogueOption)base.copyDeep<DialogueOption>();
        result.Say = Modable.copyDeep(Say);
        result.TargetStage = Modable.copyDeep(TargetStage);
        return result;
    }

    public void mod(DialogueOption modable)
    {
        base.mod(modable);
        Say = Modable.mod(Say, modable.Say);
        TargetStage = Modable.mod(TargetStage, modable.TargetStage);
    }

    public new void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((DialogueOption)modable);
    }
}

public class DialogueStage : ModableDictionary<DialogueLine>, IModable
{

    public DialogueLine Line()
    {
        return Line(GameManager.Instance.GameData);
    }

    public DialogueLine Line(GameData gameData)
    {
        IEnumerable<DialogueLine> allLines = this.Values;

        List<DialogueLine> possibleLines = new List<DialogueLine>();

        foreach (DialogueLine line in allLines)
        {
            if (line.Condition.evaluate(gameData))
                possibleLines.Add(line);
        }

        Prioritizable.Sort(possibleLines);

        if (possibleLines.Count == 0)
            throw new GameException("No valid line");

        return possibleLines[0];
    }

    public new IModable copyDeep()
    {
        return base.copyDeep<DialogueStage>();
    }


}

public class DialogueStages :  IModable
{
    public ModableDictionary<DialogueStage> stages;

    public DialogueStage StartStage()
    {
        return stages["start"];
    }

    public IModable copyDeep()
    {
        var result = new DialogueStages();

        result.stages = Modable.copyDeep(stages);

        return result;
    }

    private void mod(DialogueStages original, DialogueStages mod)
    {
        stages = Modable.mod(original.stages, mod.stages);
    }

    public void mod(DialogueStages modSublocation)
    {
        if (modSublocation == null) return;
        mod(this, modSublocation);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((DialogueStages)modable);
    }
}

public class DialogueTopic : IModable, IPrioritizable
{
    [JsonIgnore]
    public string ID;

    public bool IsEventExclusive = false;
    public bool IsGreeting = false;

    public NPCFilter NPCFilter = new NPCFilter();

    public int Priority = 0;

    [JsonProperty("Condition")]
    public string ConditionString;

    [JsonIgnore]
    public Condition Condition { get => GameManager.Instance.ConditionCache[ConditionString]; }

    public CText Title;

    public IModable copyDeep()
    {
        var result = new DialogueTopic();

        result.NPCFilter = Modable.copyDeep(NPCFilter);
        result.Priority = Modable.copyDeep(Priority);
        result.Title = Modable.copyDeep(Title);

        result.IsEventExclusive = Modable.copyDeep(IsEventExclusive);
        result.IsGreeting = Modable.copyDeep(IsGreeting);
        result.ConditionString = Modable.copyDeep(ConditionString);

        return result;
    }

    public int getPriority()
    {
        return Priority;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}

public class DialogueTopicsFile : IModable
{
    public ModableDictionary<DialogueTopic> topics = new ModableDictionary<DialogueTopic>();

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}

