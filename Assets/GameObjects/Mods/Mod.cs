using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod : IDependentObject<Mod>
{
    public string ID;
    public string Name;
    public string Version;

    [JsonProperty("Dependencies")]
    public List<string> dependencyIds = new List<string>();

    private List<Mod> _dependencies = new List<Mod>();

    public Mod(string id) {
        ID = id;
    }

    public IList<Mod> getDependencies()
    {
        return _dependencies;
    }

    public bool linkDependencies(IDictionary<string,Mod> mods)
    {
        _dependencies = new List<Mod>();

        foreach (string dependencyId in dependencyIds)
        {
            if (!mods.ContainsKey(dependencyId))
            {
                Debug.LogWarning($"Required mod {dependencyId} not found for mod {ID}");
                return false;
            }
            _dependencies.Add(mods[dependencyId]);
        }

        return true;
    }

    public void setDependencies(IEnumerable<Mod> dependencies)
    {
        _dependencies = new List<Mod>();
        foreach (Mod mod in dependencies)
        {
            _dependencies.Add(mod);
        }
    }
}

public struct ModInfo
{
    public string ID;
    public string Version;


    public ModInfo(Mod mod)
    {
        ID = mod.ID;
        Version = mod.Version;
    }
}