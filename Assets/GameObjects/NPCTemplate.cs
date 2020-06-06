using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class NPCTemplate : IModable, IInheritable
{
    [JsonProperty("Parent")]
    public string ParentID;

    [JsonIgnore]
    public NPCTemplate Parent{ get => GameManager.Instance.NPCTemplateCache[ParentID]; }

    public Value<string> GenderVisible;

    public Value<string> NameFirst;
    public Value<string> NameLast;

    public Value<int?> Age;

    public ModableObjectHashDictionary<TimeFilters> Schedules = new ModableObjectHashDictionary<TimeFilters>();

    [JsonIgnore]
    public bool inheritanceResolved = false;

    public IModable copyDeep()
    {
        var result = new NPCTemplate();

        result.NameFirst = Modable.copyDeep(NameFirst);
        result.NameLast = Modable.copyDeep(NameLast);
        result.Age = Modable.copyDeep(Age);
        result.Schedules = Modable.copyDeep(Schedules);
        result.GenderVisible = Modable.copyDeep(GenderVisible);

        return result;
    }

    public NPC generate()
    {
        /*NPC npc;

        if (String.IsNullOrEmpty(ParentID))
            npc = new NPC();
        else
            npc = GameManager.Instance.NPCTemplateCache[ParentID].generate();

        npc.nameFirst = nameFirst ?? npc.nameFirst;
        npc.nameLast  = nameLast  ?? npc.nameLast;

        if(age != null)
        {
            npc.age = (int)age;
        }*/

        NPC npc = new NPC();

        npc.NameFirst = NameFirst;
        npc.NameLast = NameLast;
        npc.age = ((int?)Age).GetValueOrDefault(30);
        npc.GenderVisible = GenderVisible;


        return npc;
    }

    public void inherit(NPCTemplate parent)
    {
        NPCTemplate parentCopy = Modable.copyDeep(parent);

        mod(parentCopy, this);

    }

    public void inherit()
    {
        NPCTemplate parent = Parent;
        if (parent != null)
            inherit(parent);

        inheritanceResolved = true;
    }


    public bool isInheritanceResolved() => inheritanceResolved;

    public void mod(NPCTemplate modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((NPCTemplate)modable);
    }

    private void mod(NPCTemplate original, NPCTemplate mod)
    {
        NameFirst = Modable.mod(original.NameFirst, mod.NameFirst);
        NameLast = Modable.mod(original.NameLast, mod.NameLast);
        Age = Modable.mod(original.Age, mod.Age);
        Schedules = Modable.mod(original.Schedules, mod.Schedules);
        GenderVisible = Modable.mod(original.GenderVisible, mod.GenderVisible);
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        inherit();
    }
}
