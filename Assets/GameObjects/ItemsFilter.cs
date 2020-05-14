using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFilter : IModable
{
    public ModableStringList Genders = new ModableStringList();
    public ModableStringList Slots = new ModableStringList();
    public ModableStringList Styles = new ModableStringList();

    public int[] Price;
    public int[] Skimpiness;

    public static IEnumerable<Item> filterSlot(ItemsCollection itemsCollection, string slot)
    {
        ItemsFilter filter = new ItemsFilter();
        filter.Slots.Add(slot);
        return filter.filter(itemsCollection);
    }

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
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

            if (Styles.Count > 0 && !Styles.Contains(item.Style))
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

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
