using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class Service : IModable, IModableAutofields
{
    public string Label;

    public CText Description = new CText();

    public int Price;
    public string Category;

    public bool StayOpen = false;

    public CommandsCollection onBuy = new CommandsCollection();

    public void buy()
    {
        GameManager.Instance.PC.moneyPay(Price);

        execute();

        if (!StayOpen)
            GameManager.Instance.UIServicesWindow.hide();
    }

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    private void execute()
    {
        foreach (Command command in onBuy.Values)
        {
            command.execute();
        }
    }
}
