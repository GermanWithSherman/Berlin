using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class StatusBarMoney : UIUpdateListener
{
    public UnityEngine.UI.Text Text;

    public void set(int money) => set((long)money);

    public void set(long money)
    {
        double m = money / 100d;
        Text.text = m.ToString("C",CultureInfo.CreateSpecificCulture("de"));
    }

    public override void uiUpdate(GameManager gameManager)
    {
        if (gameManager.UISettings.isVisibleStatusMoney())
        {
            show();
            set(gameManager.PC.moneyCash);
        }
        else
        {
            hide();
        }
    }
}
