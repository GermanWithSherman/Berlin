using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
}

