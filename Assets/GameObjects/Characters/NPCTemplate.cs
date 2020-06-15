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

    public Value<int?> BreastVolume;
    public Value<int?> HairLength;
    public Value<int?> Height;
    public Value<int?> PenisLength;


    public ModableObjectHashDictionary<TimeFilters> Schedules = new ModableObjectHashDictionary<TimeFilters>();

    [JsonIgnore]
    public bool inheritanceResolved = false;

    public IModable copyDeep()
    {
        var result = new NPCTemplate();

        result.NameFirst = Modable.copyDeep(NameFirst);
        result.NameLast = Modable.copyDeep(NameLast);
        result.Age = Modable.copyDeep(Age);
        result.BreastVolume = Modable.copyDeep(BreastVolume);
        result.HairLength = Modable.copyDeep(HairLength);
        result.Height = Modable.copyDeep(Height);
        result.PenisLength = Modable.copyDeep(PenisLength);
        result.Schedules = Modable.copyDeep(Schedules);
        result.GenderVisible = Modable.copyDeep(GenderVisible);

        return result;
    }

    public NPC generate()
    {

        NPC npc = new NPC();

        npc.NameFirst = NameFirst;
        npc.NameLast = NameLast;
        npc.age = ((int?)Age).GetValueOrDefault(30);
        npc.BodyData.GenderVisible = GenderVisible;

        npc.BodyData.BreastVolume = ((int?)BreastVolume).GetValueOrDefault(0);
        npc.BodyData.HairLength = ((int?)HairLength).GetValueOrDefault(0);
        npc.BodyData.Height = ((int?)Height).GetValueOrDefault(0);
        npc.BodyData.PenisLength = ((int?)PenisLength).GetValueOrDefault(0);

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
        BreastVolume = Modable.mod(original.BreastVolume, mod.BreastVolume);
        HairLength = Modable.mod(original.HairLength, mod.HairLength);
        Height = Modable.mod(original.Height, mod.Height);
        PenisLength = Modable.mod(original.PenisLength, mod.PenisLength);
        Schedules = Modable.mod(original.Schedules, mod.Schedules);
        GenderVisible = Modable.mod(original.GenderVisible, mod.GenderVisible);
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        inherit();
    }


}
