using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopType : IModable
{
    [JsonIgnore]
    public string ID;

    public ItemsFilter Filter;

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
