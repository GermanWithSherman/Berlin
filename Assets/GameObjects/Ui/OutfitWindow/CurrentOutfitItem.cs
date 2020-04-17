using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentOutfitItem : MonoBehaviour
{

    public OutfitWindow OutfitWindow;

    public string Slot;

    public Button Button;
    public RawImage RawImage;

    private PC currentCharacter;

    public void onClick()
    {
        OutfitWindow.itemSelectWindowShow(Slot);
    }

    public void setCharacter(PC character)
    {
        currentCharacter = character;
    }

    public void update()
    {
        Item item = currentCharacter.currentOutfit[Slot];

        RawImage.texture = item.Texture;
    }
}
