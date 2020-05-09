using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandItemAdd : Command
{
    public int Count = 1;

    public bool Equip;
    public string ItemId;

    public string ShopId = "";

    public override void execute(Data data)
    {
        ItemsCollection selectedItems;
        if (!String.IsNullOrEmpty(ShopId))
        {
            GameData gameData = (GameData)data;
            ItemsCollection items = gameData.ShopData[ShopId].ItemsAll;

            selectedItems = items.getRandomItems(Count);
        }
        else
        {
            selectedItems = new ItemsCollection(new string[] { ItemId });
        }


        foreach (Item item in ((Dictionary<string, Item>)selectedItems).Values)
        {
            GameManager.Instance.PC.itemAdd(item);
            if (Equip)
                GameManager.Instance.PC.currentOutfit.addItem(item);
        }
    }
}
