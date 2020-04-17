using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cache<T> : MonoBehaviour
{
    public int size;
    public bool hashKey;

    public T this[string key]
    {
        get { return get(key); }
    }

    private List<string> keys = new List<string>();
    private Dictionary<string,T> entries = new Dictionary<string,T>();

    protected abstract T create(string key);

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
}
