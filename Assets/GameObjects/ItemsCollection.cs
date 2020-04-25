using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        cacheDirty = false;
        foreach (Item item in items)
        {
            addItem(item);
        }
    }

    public ItemsCollection(IEnumerable<string> itemIds)
    {
        foreach (string itemId in itemIds)
        {
            Add(itemId,itemId);
        }
    }

    public new void Add(string key, string itemId)
    {
        base.Add(key,itemId);
        cacheDirty = true;
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


    public ItemsCollection getRandomItems(int count)
    {
        var result = new ItemsCollection();

        if(Count <= count)
        {
            if(Count < count)
                ErrorMessage.Show($"Requesting {count} items from {Count} available items");
            return new ItemsCollection(getItemDict().Values);
        }


        List<Item> possibleItems = getItemDict().Values.ToList();

        for(int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0,possibleItems.Count);
            Item item = possibleItems[index];

            result.addItem(item);
            possibleItems.RemoveAt(index);
        }

        return result;
    }


    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        cacheDirty = true;
    }
}
