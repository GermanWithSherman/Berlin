using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFilter
{
    public List<string> Genders = new List<string>();
    public List<string> Slots = new List<string>();
    public List<string> Types = new List<string>();

    public int[] Price;
    public int[] Skimpiness;

    public static IEnumerable<Item> filterSlot(ItemsCollection itemsCollection, string slot)
    {
        ItemsFilter filter = new ItemsFilter();
        filter.Slots.Add(slot);
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
            if (Genders.Count > 0 && !Genders.Contains(item.Gender))
                continue;

            if (Slots.Count > 0 && !Slots.Contains(item.Slot))
                continue;

            if (Types.Count > 0 && !Types.Contains(item.Type))
                continue;

            if (Price != null)
            {
                if (Price.Length == 1)
                {
                    if (item.Price != Price[0])
                        continue;
                }
                else
                {
                    if (item.Price < Price[0] || item.Price > Price[1])
                        continue;
                }
            }

            if (Skimpiness != null)
            {
                if(Skimpiness.Length == 1)
                {
                    if (item.Skimpiness != Skimpiness[0])
                        continue;
                }
                else
                {
                    if (item.Skimpiness < Skimpiness[0] || item.Skimpiness > Skimpiness[1])
                        continue;
                }
            }

            result.Add(item);
        }


        return result;
    }
}
