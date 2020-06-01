using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class SaveFile
{
    public DateTime SavegameTime;

    public string PlayerVersion;
    public IEnumerable<ModInfo> Mods = new List<ModInfo>();
    public GameData GameData;

    [OnSerializing]
    internal void OnSerializingMethod(StreamingContext context)
    {
        SavegameTime = DateTime.UtcNow;
    }
}
