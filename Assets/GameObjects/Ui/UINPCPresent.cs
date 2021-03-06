﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UINPCPresent : MonoBehaviour
{
    private Thread updateTooltipTextThread;

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

        //updateTooltipTextThread = new Thread(new ThreadStart(updateTooltipText));
        //updateTooltipTextThread.Start();
        updateTooltipText();
    }

    public void updateTooltipText()
    {
        TooltipProvider.Text = GameManager.Instance.FunctionsLibrary.npcName(_npc);
    }
}
