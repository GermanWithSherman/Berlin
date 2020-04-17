using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData : Data
{
    [JsonProperty]
    private Dictionary<string, Shop> shops = new Dictionary<string, Shop>();

    protected override dynamic get(string key)
    {
        if (!shops.ContainsKey(key))
        {
            string[] keyParts = key.Split(new char[] { '.' }, 2);
            string typeId = keyParts[0];
            

            ShopType shopType = GameManager.Instance.ShopTypeCache[typeId];
            Shop shop = new Shop(shopType);
            shops[key] = shop;
        }

        return shops[key];
    }

    protected override void set(string key, dynamic value)
    {
        throw new System.NotImplementedException();
    }
}
