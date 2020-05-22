using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageMain : UIUpdateListener
{
    //public UnityEngine.UI.RawImage RawImage;
    public ImageAutosize Image;

    public override void uiUpdate(GameManager gameManager)
    {
        Image.texture = gameManager.CurrentTexture;
        //RawImage.SizeToParent();
    }
}
