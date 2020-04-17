using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitWindow : MonoBehaviour
{
    private PC currentCharacter;

    private IEnumerable<CurrentOutfitItem> currentOutfitItems;

    public GameObject currentOutfitItemsContainer;

    public ItemSelectWindow ItemSelectWindow;

    void Start()
    {
        
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void itemSelect(Item item)
    {
        currentCharacter.currentOutfit.addItem(item);
        updateItems();
    }

    public void itemSelectWindowShow(string slot)
    {
        ItemSelectWindow.show(ItemsFilter.filterSlot(currentCharacter.items,slot), this.itemSelect);
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
        updateItems();
        gameObject.SetActive(true);
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
