using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CommandItemRemove : Command
{
    public string ItemID;
    public ItemsFilter ItemsFilter = new ItemsFilter();

    public override void execute(Data data)
    {
        if (!String.IsNullOrEmpty(ItemID))
        {
            Item item = GameManager.Instance.ItemsLibrary[ItemID];
            GameManager.Instance.PC.itemRemove(item);
        }
        GameManager.Instance.PC.itemsRemove(ItemsFilter);
    }

    public override IModable copyDeep()
    {
        var result = new CommandItemRemove();

        result.ItemID = Modable.copyDeep(ItemID);
        result.ItemsFilter = Modable.copyDeep(ItemsFilter);

        return result;
    }

    private void mod(CommandItemRemove original, CommandItemRemove mod)
    {
        ItemID = Modable.mod(original.ItemID, mod.ItemID);
        ItemsFilter = Modable.mod(original.ItemsFilter, mod.ItemsFilter);


    }

    public void mod(CommandItemRemove modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandItemRemove modCommand = modable as CommandItemRemove;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}

