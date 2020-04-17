using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTemplate
{

    public string parent;

    public Value<System.String> nameFirst;
    public Value<System.String> nameLast;

    public Value<System.Int64> age;

    public Dictionary<string, Schedule> schedules = new Dictionary<string, Schedule>();

    public NPC generate()
    {
        NPC npc;

        if (String.IsNullOrEmpty(parent))
            npc = new NPC();
        else
            npc = GameManager.Instance.NPCTemplateCache[parent].generate();

        npc.nameFirst = nameFirst ?? npc.nameFirst;
        npc.nameLast  = nameLast  ?? npc.nameLast;

        if(age != null)
        {
            npc.age = (int)age;
        }

        return npc;
    }
}
