using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShopTypeCache : Cache<ShopType>
{
    protected override ShopType create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "shoptypes", key + ".json");

        try
        {
            JObject deserializationData = GameManager.File2Data(path);

            ShopType shopType = deserializationData.ToObject<ShopType>();

            shopType.id = key;

            return shopType;
        }catch(FileNotFoundException)
        {
            throw new GameException($"ShopType {key} does not exist");
        }
    }
}
