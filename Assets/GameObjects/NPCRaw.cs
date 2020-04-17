using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRaw : NPC
{
    public NPC generate()
    {
        GameManager gameManager = GameManager.Instance;
        NPC npc = gameManager.NPCTemplateCache[templateId].generate();

        npc.nameFirst = nameFirst ?? npc.nameFirst;
        npc.nameLast = nameLast ?? npc.nameLast;

        npc.genderVisible = genderVisible ?? npc.genderVisible;

        npc.schedules = schedules ?? npc.schedules;

        npc.TexturePath = TexturePath ?? npc.TexturePath;

        return npc;
    }
}
