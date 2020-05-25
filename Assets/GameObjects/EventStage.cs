using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStage : IModable, IInheritable
{
    [JsonIgnore]
    public string StageID;

    [JsonIgnore]
    public string GroupID;

    private bool _inheritanceResolved = false;

    public CText Text = new CText();

    public CommandsCollection Commands = new CommandsCollection();

    public ModableDictionary<Option> Options = new ModableDictionary<Option> ();

    [JsonIgnore]
    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }
    public Conditional<string> TexturePath;

    public Conditional<string> Inherit = new Conditional<string>();
    public IEnumerable<string> InheritIDs
    {
        get
        {
            /*if (Inherit == null)
                return new List<string>();
            return new List<string>() { Inherit.value() };*/
            return Inherit.values();
        }
    }

    public void execute()
    {
        GameManager gameManager = GameManager.Instance;

        Commands.execute();
    }

    private void mod(EventStage original, EventStage mod)
    {

        Commands = Modable.mod(original.Commands, mod.Commands);

        Options = Modable.mod(original.Options, mod.Options);

        Text = Modable.mod(original.Text, mod.Text);

        TexturePath = Modable.mod(original.TexturePath, mod.TexturePath);
    }

    public void mod(EventStage modEventStage)
    {
        if (modEventStage == null) return;
        mod(this, modEventStage);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((EventStage)modable);
    }

    public IModable copyDeep()
    {
        var result = new EventStage();
        result.GroupID = Modable.copyDeep(GroupID);
        result.StageID = Modable.copyDeep(StageID);
        result.Commands = Modable.copyDeep(Commands);
        result.Inherit = Modable.copyDeep(Inherit);
        result.Options = Modable.copyDeep(Options);
        result.Text = Modable.copyDeep(Text);
        result.TexturePath = Modable.copyDeep(TexturePath);
        return result;
    }

    public void inherit(EventStage parent)
    {
        EventStage parentCopy = Modable.copyDeep(parent);

        mod(parentCopy, this);

    }

    public void inherit()
    {
        foreach (string InheritID in InheritIDs)
        {
            string id = InheritID;
            string[] keyParts = id.Split('.');
            if (String.IsNullOrWhiteSpace(keyParts[0]))
                id = $"{GroupID}.{keyParts[1]}";
            EventStage parent = GameManager.Instance.EventGroupCache.EventStage(id);
            if(parent != null)
                inherit(parent);
        }

        _inheritanceResolved = true;
    }

    public bool isInheritanceResolved() => _inheritanceResolved;
}
