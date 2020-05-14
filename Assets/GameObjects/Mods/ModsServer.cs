using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModsServer
{
    private string _modsDictPath;

    const string manifestFileName = "info.json";

    

    private List<string> _installedModIds = new List<string>();
    
    public List<Mod> ActivatedMods
    {
        get
        {
            return _modLoadOrder;
            /*var result = new List<Mod>();
            foreach (string id in _activatedModIds)
            {
                result.Add(_mods[id]);
            }
            return result;*/
        }
    }

    public List<string> ActivatedModsPaths
    {
        get
        {
            var result = new List<string>();
            foreach (Mod mod in _modLoadOrder)
            {
                result.Add(Path.Combine(_modsDictPath, mod.ID));
            }
            return result;
        }
    }

    private Dictionary<string, Mod> _mods = new Dictionary<string, Mod>();

    private List<Mod> _modLoadOrder = new List<Mod>();
    private List<string> _modLoadOrderIDs
    {
        get
        {
            var result = new List<string>();
            foreach (Mod mod in _modLoadOrder) { result.Add(mod.ID); }
            return result;
        }
    }

    public ModsServer(string modsDictPath)
    {
        if (!Directory.Exists(modsDictPath))
            return;

        _modsDictPath = modsDictPath;

        /*string[] modFolders  = Directory.GetDirectories(modsDictPath);

        foreach (string modFolder in modFolders)
        {
            modLoad(modFolder);
        }

        foreach(Mod mod in _mods.Values)
        {
            modLinkDependencies(mod);
        }

        _modLoadOrder = DependencySorter.Sort(ActivatedMods);

        foreach (Mod mod in _modLoadOrder)
        {
            Debug.Log($"Mod activated: {mod.ID}");
        }*/
    }

    public ModsServer(string modsDictPath, Preferences preferences)
    {
        _modsDictPath = modsDictPath;
        //_activatedModIds = preferences.ActivatedModIDs;

        string[] modFolders = Directory.GetDirectories(_modsDictPath);

        foreach (string modFolder in modFolders)
        {
            modLoad(modFolder);
        }

        //List<string> _activatedModIds = new List<string>();
        var activatedMods = new List<Mod>();

        foreach (Mod mod in _mods.Values)
        {
            if (modLinkDependencies(mod) && preferences.ActivatedModIDs.Contains(mod.ID) && !activatedMods.Contains(mod))
                //_activatedModIds.Add(mod.ID);
                activatedMods.Add(mod);
        }


        /*foreach (string id in _activatedModIds)
        {
            activatedMods.Add(_mods[id]);
        }*/

        try
        {
            _modLoadOrder = DependencySorter.Sort(activatedMods);
        }
        catch
        {
            ErrorMessage.Show("Unresolvable Error in Modlist");
            return;
        }

        foreach (Mod mod in _modLoadOrder)
        {
            Debug.Log($"Mod activated: {mod.ID}");
        }

        preferences.ActivatedModIDs = _modLoadOrderIDs;
    }

    private void modAdd(Mod mod)
    {
        if (_mods.ContainsKey(mod.ID))
        {
            Debug.LogError($"Mod ID collision: {mod.ID}");
            return;
        }
        _mods.Add(mod.ID, mod);
        _installedModIds.Add(mod.ID);
    }
    
    /*private void modActivate(Mod mod)
    {
        if (_activatedModIds.Contains(mod.ID))
            return;
        _activatedModIds.Add(mod.ID);
    }*/

    private bool modLinkDependencies(Mod mod)
    {
        /*if (mod.linkDependencies(_mods))
        {
            modActivate(mod);
        }*/
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
}
