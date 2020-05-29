using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandShop : Command
{
    public string ShopID;

    public override void execute(Data data)
    {
        GameManager.Instance.shopShow(ShopID);
    }

    public override IModable copyDeep()
    {
        var result = new CommandShop();

        result.ShopID = Modable.copyDeep(ShopID);

        return result;
    }

    private void mod(CommandShop original, CommandShop mod)
    {
        ShopID = Modable.mod(original.ShopID, mod.ShopID);

    }

    public void mod(CommandShop modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandShop modCommand = modable as CommandShop;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
