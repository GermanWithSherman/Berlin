using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiShopItem : MonoBehaviour
{
    public ImageAutosize ImageAutosize;
    public TextMeshProUGUI Text;

    private Item _item;

    public UiShopItem(Item item)
    {
        setItem(item);
    }

    public void buy()
    {
        Debug.Log($"Buy {_item.id}");
        GameManager.Instance.itemBuy(_item,_item.Price);

        update();
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void setItem(Item item)
    {
        _item = item;
        Text.text = item.Label;
        ImageAutosize.texture = item.Texture;
        update();
    }

    public void show()
    {
        gameObject.SetActive(true);
    }


    public void update()
    {
        ItemsCollection playerInventory = GameManager.Instance.PC.items;

        if (playerInventory.Contains(_item))
        {
            hide();
        }
        else
        {
            show();
        }
    }

    /// <summary> 
    /// Updates only if the containing item is the item in the parameter.
    /// </summary> 
    /// <param name="item"></param> 
    public void update(Item item)
    {
        if (item == _item)
            update();
    }
}
