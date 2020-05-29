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

    public override IModable copyDeep()
    {
        var result = new CommandItemAdd();

        result.Count = Modable.copyDeep(Count);
        result.Equip = Modable.copyDeep(Equip);
        result.ItemID = Modable.copyDeep(ItemID);
        result.ShopID = Modable.copyDeep(ShopID);
        return result;
    }

    private void mod(CommandItemAdd original, CommandItemAdd mod)
    {
        Count = Modable.mod(original.Count, mod.Count);
        Equip = Modable.mod(original.Equip, mod.Equip);

        ItemID = Modable.mod(original.ItemID, mod.ItemID);
        ShopID = Modable.mod(original.ShopID, mod.ShopID);
    }

    public void mod(CommandItemAdd modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandItemAdd modCommand = modable as CommandItemAdd;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
