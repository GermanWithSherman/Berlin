using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopType
{
    [JsonIgnore]
    public string id;

    public ItemsFilter filter;
}
