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
}
