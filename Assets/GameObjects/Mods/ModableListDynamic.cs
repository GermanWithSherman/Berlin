using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModableListDynamic : List<dynamic>, IModable
{
    public IModable copyDeep()
    {
        var result = new ModableListDynamic();

        foreach (dynamic entry in this)
        {
            result.Add(Modable.copyDeep(entry)) ;
        }

        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
