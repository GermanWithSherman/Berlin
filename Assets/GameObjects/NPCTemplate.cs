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

    public Value<System.String> genderVisible;

    public Value<System.String> nameFirst;
    public Value<System.String> nameLast;

    public Value<int?> age;

    public ModableDictionary<Schedule> Schedules = new ModableDictionary<Schedule>();

    [JsonIgnore]
    public bool inheritanceResolved = false;

    public IModable copyDeep()
    {
        var result = new NPCTemplate();

        result.nameFirst = Modable.copyDeep(nameFirst);
        result.nameLast = Modable.copyDeep(nameLast);
        result.age = Modable.copyDeep(age);
        result.Schedules = Modable.copyDeep(Schedules);
        result.genderVisible = Modable.copyDeep(genderVisible);

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

        npc.nameFirst = nameFirst;
        npc.nameLast = nameLast;
        npc.age = ((int?)age).GetValueOrDefault(30);
        npc.genderVisible = genderVisible;


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
        nameFirst = Modable.mod(original.nameFirst, mod.nameFirst);
        nameLast = Modable.mod(original.nameLast, mod.nameLast);
        age = Modable.mod(original.age, mod.age);
        Schedules = Modable.mod(original.Schedules, mod.Schedules);
        genderVisible = Modable.mod(original.genderVisible, mod.genderVisible);
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        inherit();
    }
}
