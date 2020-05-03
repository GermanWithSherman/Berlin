using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFilter : IModable
{
    public ModableDictionary<string> IDs = new ModableDictionary<string>();

    public IModable copyDeep()
    {
        var result = new NPCFilter();

        result.IDs = Modable.copyDeep(IDs);

        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public bool isValid(NPC npc)
    {
        if (IDs.Count > 0 && !IDs.ContainsValue(npc.id))
            return false;
        return true;

    }

}
