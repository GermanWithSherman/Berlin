using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class NPCFilter : IModable, IModableAutofields
{
    public ModableStringList IDs = new ModableStringList();

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
        if (IDs.Count > 0 && !IDs.Contains(npc.id))
            return false;
        return true;

    }

}
