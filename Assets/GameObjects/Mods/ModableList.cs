using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModableStringList : List<string>, IModable
{
    public IModable copyDeep()
    {
        var result = new ModableStringList();

        foreach (string s in this)
            result.Add(s);

        return result;
    }

    public void mod(IModable modable)
    {
        ModableStringList _modable = (ModableStringList)modable;

        for (int i = 0; i < Count; i++)
            this[i] = Modable.mod(this[i], _modable[i]);
    }

}
