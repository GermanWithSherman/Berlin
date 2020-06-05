using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class ModList : List<Mod>
{
    private string _modsDictPath;
    public string ModsDictPath { get => _modsDictPath; set => _modsDictPath = value; }

    public ModList() { }

    public ModList(string modsDictPath)
    {
        _modsDictPath = modsDictPath;
    }

    public IEnumerable<string> IDs
    {
        get
        {
            var result = new List<string>();
            foreach (Mod mod in this)
            {
                result.Add(mod.ID);
            }
            return result;
        }
    }

    public IEnumerable<ModInfo> ModInfos
    {
        get
        {
            var result = new List<ModInfo>();
            foreach (Mod mod in this)
            {
                result.Add(new ModInfo(mod));
            }
            return result;
        }
    }

    public IEnumerable<string> Paths
    {
        get
        {
            var result = new List<string>();
            foreach (Mod mod in this)
            {
                result.Add(Path.Combine(_modsDictPath, mod.ID));
            }
            return result;
        }
    }

    
}

public class ModsServer
{
    private string _modsDictPath;

    const string manifestFileName = "info.json";

    private Dictionary<string, Mod> _mods = new Dictionary<string, Mod>(); //All installed mods

    private ModList _modLoadOrder;

    public IEnumerable<Mod> ActivatedMods
    {
        get => _modLoadOrder;
    }

    public IEnumerable<Mod> DeactivatedMods
    {
        get
        {
            var result = new List<Mod>();
            foreach (Mod mod in _mods.Values)
            {
                if (!_modLoadOrder.Contains(mod))
                    result.Add(mod);
            }
            return result;
        }
    }

    public IEnumerable<string> ActivatedModsIDs
    {
        get => _modLoadOrder.IDs;
    }

    public IEnumerable<ModInfo> ActivatedModsInfo
    {
        get => _modLoadOrder.ModInfos;
    }

    public IEnumerable<string> ActivatedModsPaths
    {
        get => _modLoadOrder.Paths;
    }

    public ModsServer(string modsDictPath, Preferences preferences)
    {
        _modsDictPath = modsDictPath;

        _modLoadOrder = new ModList(modsDictPath);

        string[] modFolders = Directory.GetDirectories(_modsDictPath);

        foreach (string modFolder in modFolders)
        {
            modLoad(modFolder);
        }


        foreach (Mod mod in _mods.Values)
        {
            if (modDependenciesFullfilled(mod) && preferences.ActivatedModIDs.Contains(mod.ID) && !_modLoadOrder.Contains(mod))
                _modLoadOrder.Add(mod);
        }

        try
        {
            _modLoadOrder = DependencySorter.Sort<Mod, ModList>(_modLoadOrder);
            _modLoadOrder.ModsDictPath = _modsDictPath;
        }
        catch
        {
            ErrorMessage.Show("Unresolvable Error in Modlist");
            return;
        }
    }

    public ModInfo ActivatedModInfo(string modID)
    {
        if (_mods.TryGetValue(modID, out Mod mod))
        {
            if (_modLoadOrder.Contains(mod))
                return new ModInfo(mod);
        }
        return new ModInfo();
    }

    private void modAdd(Mod mod)
    {
        if (_mods.ContainsKey(mod.ID))
        {
            Debug.LogError($"Mod ID collision: {mod.ID}");
            return;
        }
        _mods.Add(mod.ID, mod);
    }

    private bool modDependenciesFullfilled(Mod mod)
    {
        return mod.linkDependencies(_mods);
    }

    private void modLoad(string path)
    {
        string manifestPath = Path.Combine(path, manifestFileName);
        if (!File.Exists(manifestPath))
        {
            Debug.LogError($"Manifest File missing in Mod Folder {path}");
            return;
        }

        JObject manifestContents = GameManager.File2Data(manifestPath);
        Mod mod = manifestContents.ToObject<Mod>();

        modAdd(mod);
    }

    /*


    public ModsServer(string modsDictPath, Preferences preferences)
    {
        _modsDictPath = modsDictPath;



        var activatedMods = new List<Mod>();

        foreach (Mod mod in Mods.Values)
        {
            if (modLinkDependencies(mod) && preferences.ActivatedModIDs.Contains(mod.ID) && !activatedMods.Contains(mod))
                activatedMods.Add(mod);
        }



        

        foreach (Mod mod in ActivatedMods)
        {
            mod.Activated = true;
            Debug.Log($"Mod activated: {mod.ID}");
        }

        preferences.ActivatedModIDs = _modLoadOrderIDs;
    }

    

    


    

    */
}
