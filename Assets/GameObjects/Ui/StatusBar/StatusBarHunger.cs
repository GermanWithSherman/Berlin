using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHunger : UIUpdateListener
{
    public TMPro.TextMeshProUGUI Text;

    public void set(long hunger)
    {
        decimal h = hunger / 1000000m;
        Text.text = h.ToString("P");
    }

    public override void uiUpdate(GameManager gameManager)
    {
        set(gameManager.PC.statHunger);
    }
}
