using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentOutfitItem : MonoBehaviour
{

    public OutfitWindow OutfitWindow;

    public string Slot;

    public string Label;

    public TMPro.TextMeshProUGUI LabelText;

    public Button Button;
    public RawImage RawImage;

    private PC currentCharacter;

    void Start()
    {
        LabelText.text = Label;
    }

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

        if(item == null)
        {
            RawImage.texture = currentCharacter.GetBodypartTexture(Slot);
            return;
        }
        RawImage.texture = item.Texture;
    }
}
