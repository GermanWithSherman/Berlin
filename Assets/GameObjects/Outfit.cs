using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outfit : IModable
{
    [JsonProperty]
    private ItemsCollection items = new ItemsCollection();

    [JsonIgnore]
    public IEnumerable<Item> Items { get => ((Dictionary<string, Item>)items).Values; }

    public string Gender = "None";
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
        items.removeItem(item);
        //items.setItem(item.Slot, (Item)null);
    }

    public void setItem(string slot, Item item)
    {
        items.setItem(slot,item);
        update();
    }

    public void setItem(string slot, string itemId)
    {
        items.setItem(slot, itemId);
        
    }

    private void update()
    {
        updateGender();
        updateStyle();
    }

    private void updateGender()
    {
        Item bra = items.getItem("Bra");
        Item clothes = items.getItem("Clothes");
        Item panties = items.getItem("Panties");
        Item shoes = items.getItem("Shoes");

        Item[] compareItems;

        if (clothes == null)
            compareItems = new Item[] { bra,panties,shoes };
        else
            compareItems = new Item[] { clothes, shoes };

        int fm_counter = 0;
        int f_counter = 0;
        int m_counter = 0;

        foreach (Item item in compareItems)
        {
            if (item == null)
                continue;
            if (item.Gender == "fm") fm_counter++;
            else if (item.Gender == "f") f_counter++;
            else if (item.Gender == "m") m_counter++;
        }

        if(fm_counter == 0 && f_counter == 0 && m_counter == 0)
        {
            Gender = "None";
            return;
        }

        // FM F M
        if(m_counter == 0)
        {
            //? ? 0
            if (f_counter == 0)
                //? 0 0
                Gender = "fm";
            else
                //? 1 0
                Gender = "f";
        }
        else
        {
            //? ? 1
            if (f_counter == 0)
                //? 0 1
                Gender = "m";
            else
                //? 1 1
                Gender = "mix";
        }
        Debug.Log($"New utfit gender: {Gender}");
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
        if (panties != null)
            pantiesStyle = panties.Style.ToArray();
        if (shoes != null)
            shoesStyle = shoes.Style.ToArray();

        if (clothes != null)
            clothesStyle = clothes.Style.ToArray();
        else if (panties != null)
            clothesStyle = new string[] { "Underwear" };

        FunctionParameters parameters = new FunctionParameters();
        parameters.Add("_bra", braStyle);
        parameters.Add("_clothes", clothesStyle);
        parameters.Add("_panties", pantiesStyle);
        parameters.Add("_shoes", shoesStyle);

        string style = GameManager.Instance.FunctionsLibrary.FunctionExecute("s_OutfitStyle",GameManager.Instance.GameData, parameters);

        Style = style;
        Debug.Log($"New style: {style}");
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public IModable copyDeep()
    {
        var result = new Outfit();
        result.items = Modable.copyDeep(items);
        result.update(); //could be more efficient by copying values directly
        return result;
    }
}
