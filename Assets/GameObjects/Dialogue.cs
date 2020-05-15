using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine : IModable, IPrioritizable
{
    public string TopicID;

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

        result.TopicID = Modable.copyDeep(TopicID);
        result.Priority = Modable.copyDeep(Priority);
        result.Options = Modable.copyDeep(Options);
        result._text = Modable.copyDeep(_text);

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

    public string Text(Data data)
    {
        return _text.Text(data);
    }
}

public class DialogueOption : Option, IModable
{
    public string TargetStage;

    public new IModable copyDeep()
    {
        DialogueOption result = (DialogueOption)base.copyDeep<DialogueOption>();
        result.TargetStage = Modable.copyDeep(TargetStage);
        return result;
    }

    public void mod(DialogueOption modable)
    {
        base.mod(modable);
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

public class DialogueStages : ModableDictionary<DialogueStage>, IModable
{
    public DialogueStage StartStage()
    {
        return this["start"];
    }

    public new IModable copyDeep()
    {
        return base.copyDeep<DialogueStages>();
    }
}

public class DialogueTopic : IModable, IPrioritizable
{
    [JsonIgnore]
    public string ID;

    public bool IsGreeting = false;

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

    public int getPriority()
    {
        return Priority;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
