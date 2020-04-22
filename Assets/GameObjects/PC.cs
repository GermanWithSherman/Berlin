﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PC : NPC
{
    /*[JsonIgnore]
    public List<Item> items = new List<Item>();

    private List<string> _itemIds = new List<string>(); //for deserialization only
    public List<string> itemIds
    {
        get => itemIdsGet();
        set => _itemIds = value;
    }*/

    public ItemsCollection items = new ItemsCollection();

    public Dictionary<string, Outfit> outfits = new Dictionary<string, Outfit>();
    public string currentOutfitId;
    [JsonIgnore]
    public Outfit currentOutfit
    {
        get => outfits[currentOutfitId];
    }


    public Int64 moneyCash = 0;

    public int statCalories;
    public int statCaloriesBurnt;

    public int statHunger;
    private int _statHunger { get => statHunger; set => statHunger = Mathf.Clamp(value, 0, 1000000); }


    public int statSleep;
    private int _statSleep { get => statSleep; set => statSleep = Mathf.Clamp(value, 0, 1000000); }



    


    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "moneyCash":
                return moneyCash;
            case "statHunger":
                return statHunger;
            case "statSleep":
                return statSleep;
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
            case "statHunger":
                statHunger = value;
                return;
            case "statSleep":
                statSleep = value;
                return;
        }

        setNPCData(key, value);
    }

    public void itemAdd(Item item)
    {
        if(!items.Contains(item))
            items.setItem(item.id,item);
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

        weight += weightChange;

        statCalories = 0;
        statCaloriesBurnt = 0;

    }


    public void timePass(int seconds, Activity activity)
    {
        _statHunger += seconds * activity.statHunger;
        _statSleep += seconds * activity.statSleep;
    }


}
