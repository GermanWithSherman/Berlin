using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UINPCsPresentContainer : MonoBehaviour
{
    public UINPCPresent NPCPrefab;

    private IEnumerable<NPC> _npcs;

    private void clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void setNPCs(IEnumerable<NPC> npcs)
    {
        if(_npcs != null && npcs.SequenceEqual(_npcs, new NPCComparer()))
        {
            Debug.Log("Skipped NPC update");
            return;
        }

        _npcs = npcs;

        clear();

        foreach (NPC npc in npcs)
        {
            UINPCPresent uiNPC = Instantiate(NPCPrefab, transform);
            uiNPC.setNPC(npc);
        }
    }
}
