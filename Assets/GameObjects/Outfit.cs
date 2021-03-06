﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outfit
{
    [JsonProperty]
    private ItemsCollection items = new ItemsCollection();

    public string Style = "Casual";

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

    public dynamic getDynamic(string key)
    {
        string[] keyparts = key.Split(new char[] { '.' }, 2);

        if (keyparts.Length == 1)
            return this[key];

        return this[keyparts[0]]?.getDynamic(keyparts[1]);
    }

    public void removeItem(Item item)
    {
        items.setItem(item.Slot, (Item)null);
    }

    public void setItem(string slot, Item item)
    {
        items.setItem(slot,item);
        updateStyle();
    }

    public void setItem(string slot, string itemId)
    {
        items.setItem(slot, itemId);
        updateStyle();
    }

    private void updateStyle()
    {
        string[] braStyle = new string[] { "None" };
        string[] clothesStyle = new string[] { "None" };
        string[] pantiesStyle = new string[] { "None" };
        string[] shoesStyle = new string[] { "None" };

        Item bra = items.getItem("Bra");
        Item clothes = items.getItem("Clothes");
        Item panties = items.getItem("Panties");
        Item shoes = items.getItem("Shoes");

        if (bra != null)
            braStyle = bra.Style.ToArray();
        if (clothes != null)
            clothesStyle = clothes.Style.ToArray();
        if (panties != null)
            pantiesStyle = panties.Style.ToArray();
        if (shoes != null)
            shoesStyle = shoes.Style.ToArray();

        FunctionParameters parameters = new FunctionParameters();
        parameters.Add("_bra", braStyle);
        parameters.Add("_clothes", clothesStyle);
        parameters.Add("_panties", pantiesStyle);
        parameters.Add("_shoes", shoesStyle);

        string style = GameManager.Instance.FunctionsLibrary.functionExecute("s_OutfitStyle", parameters);

        Style = style;
        Debug.Log(style);
    }
}
