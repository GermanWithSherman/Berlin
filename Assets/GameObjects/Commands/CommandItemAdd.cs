using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandItemAdd : Command
{
    public int Count = 1;

    public bool Equip;
    public string ItemID;

    public string ShopID = "";

    public override void execute(Data data)
    {
        ItemsCollection selectedItems;
        if (!String.IsNullOrEmpty(ShopID))
        {
            GameData gameData = (GameData)data;
            ItemsCollection items = gameData.ShopData[ShopID].ItemsAll;

            selectedItems = items.getRandomItems(Count);
        }
        else
        {
            selectedItems = new ItemsCollection(new string[] { ItemID });
        }


        foreach (Item item in ((Dictionary<string, Item>)selectedItems).Values)
        {
            GameManager.Instance.PC.itemAdd(item);
            if (Equip)
                GameManager.Instance.PC.currentOutfit.addItem(item);
        }
    }
}
