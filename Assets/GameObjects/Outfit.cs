using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outfit
{
    [JsonProperty]
    private ItemsCollection items = new ItemsCollection();

    public Item this[string slot]
    {
        get => items.getItem(slot);
    }

    public Outfit() { }

    public Outfit(IEnumerable<Item> items)
    {
        foreach(Item item in items)
        {
            addItem(item);
        }
    }

    public Outfit(IDictionary<string,string> itemIds)
    {
        foreach (KeyValuePair<string,string> kv in itemIds)
        {
            setItem(kv.Key,kv.Value);
        }
    }

    

    public void addItem(Item item)
    {
        setItem(item.Slot,item);
    }

    public void setItem(string slot, Item item)
    {
        items.setItem(slot,item);
    }

    public void setItem(string slot, string itemId)
    {
        items.setItem(slot, itemId);
    }
}
