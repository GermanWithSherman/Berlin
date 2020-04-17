using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFilter
{
    public List<string> allowedSlots = new List<string>();

    public static IEnumerable<Item> filterSlot(ItemsCollection itemsCollection, string slot)
    {
        ItemsFilter filter = new ItemsFilter();
        filter.allowedSlots.Add(slot);
        return filter.filter(itemsCollection);
    }

    public IEnumerable<Item> filter(ItemsCollection itemsCollection)
    {
        IEnumerable<Item> items = itemsCollection.getItems();

        return filter(items);

        
    }

    public IEnumerable<Item> filter(IEnumerable<Item> items)
    {
        var result = new List<Item>();

        foreach (Item item in items)
        {
            if (allowedSlots.Count > 0 && !allowedSlots.Contains(item.Slot))
                continue;

            result.Add(item);
        }


        return result;
    }
}
