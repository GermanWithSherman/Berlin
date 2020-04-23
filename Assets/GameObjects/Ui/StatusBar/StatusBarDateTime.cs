using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarDateTime : UIUpdateListener
{
    public UnityEngine.UI.Text Text;

    public override void uiUpdate(GameManager gameManager)
    {
        Text.text = gameManager.GameData.WorldData.DateTime.ToString();
    }
}
