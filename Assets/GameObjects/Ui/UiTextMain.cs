using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiTextMain : UIUpdateListener
{
    public TextMeshProUGUI Text;

    public override void uiUpdate(GameManager gameManager)
    {
        Text.text = gameManager.CurrentText;
    }
}
