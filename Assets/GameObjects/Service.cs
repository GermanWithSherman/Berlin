using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service
{
    public string label;
    public int price;
    public string category;

    public Dictionary<string, Command> onBuy = new Dictionary<string, Command>();

    public void buy()
    {
        GameManager.Instance.PC.moneyPay(price);

        execute();
    }

    private void execute()
    {
        foreach (Command command in onBuy.Values)
        {
            command.execute();
        }
    }
}
