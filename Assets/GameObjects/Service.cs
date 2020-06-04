using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service
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

    private void execute()
    {
        foreach (Command command in onBuy.Values)
        {
            command.execute();
        }
    }
}
