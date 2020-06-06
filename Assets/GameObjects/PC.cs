using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class PC : NPC
{

    public ItemsCollection items = new ItemsCollection();

    public Dictionary<string,Outfit> outfits = new Dictionary<string, Outfit>();
    public string currentOutfitId="DEFAULT";
    [JsonIgnore]
    public Outfit currentOutfit
    {
        get
        {
            if (!outfits.ContainsKey(currentOutfitId))
                outfits[currentOutfitId] = new Outfit();
            return outfits[currentOutfitId];
        }
    }

    public string nameFirstBorn;
    public string nameLastBorn;


    public Int64 moneyCash = 0;

    public int statCalories;
    public int statCaloriesBurnt;

    public int statHunger;
    private int _statHunger { get => statHunger; set => statHunger = Mathf.Clamp(value, 0, 1000000); }

    public int statHygiene;
    private int _statHygiene { get => statHygiene; set => statHygiene = Mathf.Clamp(value, 0, 1000000); }

    public int statSleep;
    private int _statSleep { get => statSleep; set => statSleep = Mathf.Clamp(value, 0, 1000000); }

    public override bool ShouldSerializeTexturePath() => true;

    private static GameManager gameManager { get => GameManager.Instance; }



    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "moneyCash":
                return moneyCash;
            case "nameFirstBorn":
                return nameFirstBorn;
            case "nameLastBorn":
                return nameLastBorn;
            case "statHunger":
                return statHunger;
            case "statHygiene":
                return statHygiene;
            case "statSleep":
                return statSleep;
        }

        string[] keyparts = key.Split(new char[] { '.' }, 2);

        if (keyparts.Length == 2)
        { 
            switch (keyparts[0])
            {
                case "currentOutfit":
                    return currentOutfit.getDynamic(keyparts[1]);
            }
        }


        return base.get(key);
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "moneyCash":
                moneyCash = value;
                return;
            case "nameFirstBorn":
                nameFirstBorn = value;
                return;
            case "nameLastBorn":
                nameLastBorn = value;
                return;
            case "statHunger":
                _statHunger = (int)value;
                return;
            case "statHygiene":
                _statHygiene = (int)value;
                return;
            case "statSleep":
                _statSleep = (int)value;
                return;
        }

        setNPCData(key, value);
    }

    public Texture GetClothingslotTexture(string slot)
    {
        string path = "";
        switch (slot)
        {
            case "Bra":
                Item bra = currentOutfit["Bra"];
                if (bra == null)
                {
                    path = gameManager.FunctionsLibrary.functionExecute("s_PcBodyBreast", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return bra.Texture;
            case "Clothes":
                Item clothes = currentOutfit["Clothes"];
                if (clothes == null)
                    return GetClothingslotTexture("Bra");
                return clothes.Texture;
            case "Panties":
                Item panties = currentOutfit["Panties"];
                if (panties == null) {
                    path = gameManager.FunctionsLibrary.functionExecute("s_PcBodyLap", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return panties.Texture;
            case "Shoes":
                Item shoes = currentOutfit["Shoes"];
                if (shoes == null)
                {
                    path = gameManager.FunctionsLibrary.functionExecute("s_PcBodyFeet", new FunctionParameters("_PC", this, "_slot", slot));
                    return gameManager.TextureCache[path];
                }
                return shoes.Texture;
        }

        return gameManager.TextureCache[path];

    }

    public void itemAdd(Item item)
    {
        if(!items.Contains(item))
            items.setItem(item.id,item);
    }

    public void itemRemove(Item item)
    {
        items.removeItem(item);
        foreach(Outfit outfit in outfits.Values)
        {

        }
    }

    public void itemsRemove(ItemsFilter itemsFilter)
    {
        /*List<Item> itemsToRemove = new List<Item>();
        foreach(Item item in ((Dictionary<string, Item>)items).Values)
        {
            if(itemsFilter.filter)
        }*/
        IEnumerable<Item> itemsToRemove = itemsFilter.filter(items);

        foreach (Item item in itemsToRemove)
        {
            itemRemove(item);
        }
    }

    public bool itemHas(Item item)
    {
        return items.Contains(item);
    }

    private IEnumerable<string> itemIdsGet()
    {
        return items.Values;
    }

    public void moneyPay(long amount)
    {
        moneyCash -= amount;
        GameManager.Instance.uiUpdate();
    }

    public void timeDayPass()
    {
        //TODO: actual calculation required
        int caloriesRequired = 2000;

        int caloriesRequiredNet = caloriesRequired + statCaloriesBurnt;

        int caloriesToLoseWeightFast = Mathf.RoundToInt(caloriesRequiredNet * 0.6f);
        int caloriesToLoseWeight = Mathf.RoundToInt(caloriesRequiredNet * 0.8f);

        int caloriesToGainWeight = Mathf.RoundToInt(caloriesRequiredNet * 1.25f);
        int caloriesToGainWeightFast = Mathf.RoundToInt(caloriesRequiredNet * 1.67f);

        int weightChange = 0;

        if (statCalories > caloriesToGainWeightFast)
            weightChange = 140;
        else if (statCalories > caloriesToGainWeight)
            weightChange = 70;
        else if (statCalories < caloriesToLoseWeightFast)
            weightChange = -140;
        else if (statCalories < caloriesToLoseWeight)
            weightChange = -70;

        Weight += weightChange;

        statCalories = 0;
        statCaloriesBurnt = 0;

    }


    public void timePass(int seconds, Activity activity)
    {
        _statHunger += seconds * activity.statHunger;
        _statHygiene += seconds * activity.statHygiene;
        _statSleep += seconds * activity.statSleep;
    }


}
