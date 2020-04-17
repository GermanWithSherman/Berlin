using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectWindow : MonoBehaviour
{
    public delegate void ItemCallback(Item item);

    public ItemSelectWindowItem ItemPrefab;
    public Transform ItemsContainer;

    private ItemCallback _callback;

    private void deleteItems()
    {
        foreach (Transform child in ItemsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void itemSelect(Item item)
    {
        hide();
        _callback(item);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void show(ItemsCollection itemsCollection, ItemCallback callback)
    {
        show(itemsCollection.getItems(),callback);
    }

    public void show(IEnumerable<Item> items, ItemCallback callback)
    {
        _callback = callback;

        deleteItems();

        show();

        //TODO: Filter

        foreach(Item item in items)
        {
            ItemSelectWindowItem uiItem = Instantiate(ItemPrefab,ItemsContainer);
            uiItem.setItem(item,this);
        }
    }
}
