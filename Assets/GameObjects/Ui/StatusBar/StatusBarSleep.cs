using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarSleep : UIUpdateListener
{
    public TMPro.TextMeshProUGUI Text;

    public void set(long sleep)
    {
        decimal h = sleep / 1000000m;
        Text.text = h.ToString("P");
    }

    public override void uiUpdate(GameManager gameManager)
    {
        if (gameManager.UISettings.isVisibleStatusSleep())
        {
            show();
            set(gameManager.PC.statSleep);
        }
        else
        {
            hide();
        }
    }
}
