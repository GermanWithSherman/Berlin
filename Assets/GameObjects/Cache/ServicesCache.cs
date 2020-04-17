using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ServicesCache : Cache<ServicesFile>
{
    protected override ServicesFile create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "services", key + ".json");

        JObject deserializationData = GameManager.File2Data(path);

        ServicesFile servicesFile = deserializationData.ToObject<ServicesFile>();

        //shopType.id = key;

        return servicesFile;
    }

    public override ServicesFile get(string key)
    {
        string[] keyParts = key.Split('.');

        if(keyParts.Length == 1)
            return base.get(key);
        return base.get(keyParts[0]);
    }

    public Service service(string key)
    {
        string[] keyParts = key.Split('.');
        return get(keyParts[0])[keyParts[1]];
    }
}
