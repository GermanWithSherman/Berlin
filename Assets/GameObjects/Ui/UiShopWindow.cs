using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiShopWindow : MonoBehaviour
{
    public Transform itemsContainer;
    public UiShopItem itemPrefab;

    public void close()
    {
        gameObject.SetActive(false);
    }

    private void deleteItems()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void show(Shop shop)
    {
        

        deleteItems();

        gameObject.SetActive(true);

        foreach (Item item in shop.Items.getItems())
        {
            UiShopItem uiItem = Instantiate<UiShopItem>(itemPrefab, itemsContainer);
            uiItem.setItem(item);
        }
    }
}
