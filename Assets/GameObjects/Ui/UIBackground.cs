using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : UIUpdateListener
{
    public Image Image;

    public override void uiUpdate(GameManager gameManager)
    {
        Template template = gameManager.CurrentTemplate;

        Image.color = template.BackgroundColor;
    }
}
