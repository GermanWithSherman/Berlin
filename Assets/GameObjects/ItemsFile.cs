using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemsFile : IModable
{
    public ModableDictionary<Item> Items = new ModableDictionary<Item>();

    public IModable copyDeep()
    {
        var result = new ItemsFile();
        result.Items = Modable.copyDeep(Items);
        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
