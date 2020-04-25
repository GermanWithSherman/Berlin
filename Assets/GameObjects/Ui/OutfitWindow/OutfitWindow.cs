﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitWindow : MonoBehaviour
{
    private PC currentCharacter;

    private IEnumerable<CurrentOutfitItem> currentOutfitItems;

    public GameObject currentOutfitItemsContainer;

    public ItemSelectWindow ItemSelectWindow;

    private string _itemSelectionSlot = "";

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void itemSelect(Item item)
    {
        if (item == null)
            currentCharacter.currentOutfit.setItem(_itemSelectionSlot, item);
        else
            currentCharacter.currentOutfit.addItem(item);
        updateItems();
    }

    public void itemSelectWindowShow(string slot)
    {
        ItemSelectWindow.show(ItemsFilter.filterSlot(currentCharacter.items,slot), this.itemSelect);
        _itemSelectionSlot = slot;
    }

    public void setCharacter(PC character)
    {
        currentCharacter = character;

        currentOutfitItems = currentOutfitItemsContainer.transform.GetComponentsInChildren<CurrentOutfitItem>();

        foreach (CurrentOutfitItem currentOutfitItem in currentOutfitItems)
        {
            currentOutfitItem.setCharacter(currentCharacter);
            currentOutfitItem.OutfitWindow = this;
        }
    }

    public void show()
    {
        gameObject.SetActive(true);
        updateItems();
        
    }

    public void update()
    {
        updateItems();
    }

    private void updateItems()
    {
        foreach (CurrentOutfitItem currentOutfitItem in currentOutfitItems)
        {
            currentOutfitItem.update();
        }
    }
}
