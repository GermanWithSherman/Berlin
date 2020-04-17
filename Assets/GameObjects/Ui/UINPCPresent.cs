using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINPCPresent : MonoBehaviour
{
    public RawImage Texture;
    public TooltipProvider TooltipProvider;

    public void setNPC(NPC npc)
    {
        Texture.texture = npc.Texture;
        TooltipProvider.Text = GameManager.Instance.FunctionsLibrary.npcName(npc);
    }
}
