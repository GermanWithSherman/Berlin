using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectWindowItem : MonoBehaviour
{
    private Item _item;
    private ItemSelectWindow _itemSelectWindow;

    public RawImage RawImage;
    public Text Text;

    public void onClick()
    {
        _itemSelectWindow.itemSelect(_item);
    }

    public void setItem(Item item, ItemSelectWindow itemSelectWindow)
    {
        _item = item;
        _itemSelectWindow = itemSelectWindow;

        RawImage.texture = item.Texture;
        Text.text = item.Label;
    }
}
