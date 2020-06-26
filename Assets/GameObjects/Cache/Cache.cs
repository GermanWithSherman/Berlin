﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Cache<T> : MonoBehaviour where T : class,IModable,new()
{
    public string Folder;

    public int size;
    public bool hashKey;

    public T this[string key]
    {
        get { return get(key); }
    }

    private List<string> keys = new List<string>();
    private Dictionary<string,T> _strongReferences = new Dictionary<string,T>();
    private Dictionary<string, WeakReference<T>> _weakReferences = new Dictionary<string, WeakReference<T>>();

    private Dictionary<string, T> _editData = new Dictionary<string, T>();

    public void SetEditData(string key, T data)
    {
        _editData[key] = data;
    }

    public void Reset()
    {
        _strongReferences.Clear();
        _weakReferences.Clear();
        keys.Clear();
    }

    protected virtual T create(string key)
    {
        return loadFromFileWithMods(key);
    }


    private void strongReferencesCrop()
    {
        if (keys.Count >= size)
        {
            string keyToRemove = keys[0];
            _strongReferences.Remove(keyToRemove);
            keys.RemoveAt(0);
        }
    }

    private void strongReferencesPromote(string key)
    {
        int indexOfKey = keys.IndexOf(key);
        if (indexOfKey >= 0)
        {
            keys.RemoveAt(indexOfKey);
            keys.Add(key);
        }
        else
        {
            keys.Add(key);
            strongReferencesCrop();
        }
    }

    protected void add(string key, T value)
    {
        try
        {
            if (!_strongReferences.ContainsKey(key))
                _strongReferences.Add(key, value);
            strongReferencesPromote(key);
            if (!_weakReferences.ContainsKey(key))
                _weakReferences[key] = new WeakReference<T>(value);
            else
                _weakReferences[key].SetTarget(value);
        }
        catch(Exception)
        {
            Debug.LogError($"Error Adding {key} to {GetType()}");
        }
    }


    

    public virtual T get(string key)
    {
        string treatedKey = key;

        if (String.IsNullOrEmpty(key))
            return getInvalidKeyEntry(key);

        if (hashKey)
            treatedKey = System.Convert.ToString(key.GetHashCode());

        T result;

        if (_strongReferences.ContainsKey(treatedKey))
        {
            strongReferencesPromote(treatedKey);
            return _strongReferences[treatedKey];
        }
        else if (_weakReferences.ContainsKey(treatedKey))
        {
            
            if(_weakReferences[treatedKey].TryGetTarget(out result))
            {
                add(treatedKey,result);
                return result;
            }
            else
            {
                result = create(key);
                add(treatedKey, result);
                return result;
            }

        }
        else
        {
            result = create(key);
            add(treatedKey, result);
            return result;
        }


    }

    protected virtual T getInvalidKeyEntry(string key)
    {
        throw new NotImplementedException($"Handling of invalid key {key} not implemented");
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
            T mod = loadFromFile(path);
            original = Modable.mod(original, mod);
            
        }

        if (_editData.TryGetValue(key, out T editData))
            original = Modable.mod(original, editData);

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
            Debug.LogError(e);
            return new T();
        }
    }
}
