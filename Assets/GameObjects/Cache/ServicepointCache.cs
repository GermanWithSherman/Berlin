using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ServicepointCache : Cache<Servicepoint>
{
    protected override Servicepoint create(string key)
    {
        string path = Path.Combine(GameManager.Instance.DataPath, "servicepoints", key + ".json");

        JObject deserializationData = GameManager.File2Data(path);

        Servicepoint servicepoint = deserializationData.ToObject<Servicepoint>();

        return servicepoint;
    }
}
