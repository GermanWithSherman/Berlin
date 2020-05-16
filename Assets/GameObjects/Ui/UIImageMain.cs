using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageMain : UIUpdateListener
{
    public UnityEngine.UI.RawImage RawImage;

    public override void uiUpdate(GameManager gameManager)
    {
        RawImage.texture = gameManager.CurrentTexture;
        RawImage.SizeToParent();
    }
}
