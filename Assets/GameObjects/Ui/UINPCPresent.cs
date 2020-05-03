using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINPCPresent : MonoBehaviour
{
    private NPC _npc;

    public RawImage Texture;
    public TooltipProvider TooltipProvider;

    public void onClick()
    {
        GameManager.Instance.dialogueShow(_npc);
    }

    public void setNPC(NPC npc)
    {
        _npc = npc;

        Texture.texture = npc.Texture;
        TooltipProvider.Text = GameManager.Instance.FunctionsLibrary.npcName(npc);
    }
}
