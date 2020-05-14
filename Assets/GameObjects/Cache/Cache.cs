using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Cache<T> : MonoBehaviour where T : IModable
{
    public string Folder;

    public int size;
    public bool hashKey;

    public T this[string key]
    {
        get { return get(key); }
    }

    private List<string> keys = new List<string>();
    private Dictionary<string,T> entries = new Dictionary<string,T>();

    protected virtual T create(string key)
    {
        return loadFromFileWithMods(key);
    }

    private void add(string key)
    {
        string data = key;

        if (hashKey)
            key = System.Convert.ToString(key.GetHashCode());

        if (keys.Count >= size)
        {
            string keyToRemove = keys[0];
            entries.Remove(keyToRemove);
            keys.RemoveAt(0);
        }
        entries.Add(key, create(data));
        keys.Add(key);
    }

    

    public virtual T get(string key)
    {
        if (String.IsNullOrEmpty(key))
            return getInvalidKeyEntry(key);

        string data = key;
        if (hashKey)
            key = System.Convert.ToString(key.GetHashCode());
        int indexOfKey = keys.IndexOf(key);
        if (indexOfKey >= 0)
        {
            keys.RemoveAt(indexOfKey);
            keys.Add(key);
            return entries[key];
        }
        else
        {
            add(data);
            return entries[key];
        }

    }

    protected virtual T getInvalidKeyEntry(string key)
    {
        throw new NotImplementedException();
    }

    protected T loadFromFileWithMods(string key)
    {
        string dataPath = GameManager.Instance.DataPath;
        IEnumerable<string> modsPaths = GameManager.Instance.ModsServer.ActivatedModsPaths;
        return loadFromFileWithMods(key, dataPath, modsPaths);
    }

    protected T loadFromFileWithMods(string key, string dataPath, IEnumerable<string> modsPaths)
    {
        string path = Path.Combine(dataPath, Folder, key + ".json");
        T original = loadFromFile(path);

        foreach (string modPath in modsPaths) {
            path = Path.Combine(modPath, Folder, key + ".json");
            original = Modable.mod(original, loadFromFile(path));
        }

        return original;
    }

    protected T loadFromFile(string path)
    {

        try
        {
            JObject data = GameManager.File2Data(path);

            T obj = data.ToObject<T>();

            return obj;
        }
        catch (Exception e)
        {
            //Debug.LogError(e);
            return default;
        }
    }
}
