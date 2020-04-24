using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ItemsCollection : Dictionary<string,string>
{

    private Dictionary<string, Item> cache = new Dictionary<string, Item>();
    private bool cacheDirty = false;


    public static implicit operator Dictionary<string, Item>(ItemsCollection itemsCollection)
    {
        return itemsCollection.getItemDict();
    }

    public ItemsCollection() { }

    public ItemsCollection(IEnumerable<Item> items) {
        foreach (Item item in items)
        {
            addItem(item);
        }
    }

    public bool Contains(Item item)
    {
        return this.ContainsValue(item.id);
    }

    public void addItem(Item item)
    {
        setItem(item.id,item);
    }

    public Item getItem(string slot)
    {
        Dictionary<string, Item> dict = getItemDict();
        return dict.ContainsKey(slot) ? dict[slot] : null;
    }

    private Dictionary<string,Item> getItemDict()
    {
        if (!cacheDirty)
            return cache;

        ItemsLibrary itemsLibrary = GameManager.Instance.ItemsLibrary;

        cache = new Dictionary<string, Item>();

        foreach (KeyValuePair<string,string> kv in this)
        {
            cache.Add(kv.Key,itemsLibrary[kv.Value]);
        }

        cacheDirty = false;
        return cache;
    }

    public IEnumerable<Item> getItems()
    {
        return getItemDict().Values;
    }

    public void setItem(string key, Item item)
    {
        if (item == null)
        {
            Remove(key);

            if (!cacheDirty)
                cache.Remove(key);
        }
        else
        {

            this[key] = item.id;

            if (!cacheDirty)
                cache[key] = item;
        }
    }

    public void setItem(string key, string itemId)
    {
        this[key] = itemId;
        cacheDirty = true;
    }


    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        cacheDirty = true;
    }
}
