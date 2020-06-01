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
    public int[] Height;

    public static IEnumerable<Item> filterSlot(ItemsCollection itemsCollection, string slot)
    {
        ItemsFilter filter = new ItemsFilter();
        filter.Slots.Add(slot);
        return filter.filter(itemsCollection);
    }

    public IModable copyDeep()
    {
        var result = new ItemsFilter();

        result.Genders = Modable.copyDeep(Genders);
        result.Slots = Modable.copyDeep(Slots);
        result.Styles = Modable.copyDeep(Styles);
        result.Price = Modable.copyDeep(Price);
        result.Skimpiness = Modable.copyDeep(Skimpiness);
        result.Height = Modable.copyDeep(Height);

        return result;
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

            if (Styles.Count > 0)
            {
                bool styleMatched = false;
                foreach(string style in Styles)
                {
                    if (item.Style.Contains(style))
                    {
                        styleMatched = true;
                        break;
                    }
                }
                if (!styleMatched)
                    continue;
            }

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

            if (Height != null)
            {
                if (Height.Length == 1)
                {
                    if (item.Height != Height[0])
                        continue;
                }
                else
                {
                    if (item.Height < Height[0] || item.Height > Height[1])
                        continue;
                }
            }

            result.Add(item);
        }


        return result;
    }

    private void mod(ItemsFilter original, ItemsFilter mod)
    {
        Genders = Modable.mod(original.Genders, mod.Genders);
        Slots = Modable.mod(original.Slots, mod.Slots);
        Styles = Modable.mod(original.Styles, mod.Styles);
        Price = Modable.mod(original.Price, mod.Price);
        Skimpiness = Modable.mod(original.Skimpiness, mod.Skimpiness);
        Height = Modable.mod(original.Height, mod.Height);
    }

    public void mod(ItemsFilter modSublocation)
    {
        if (modSublocation == null) return;
        mod(this, modSublocation);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((ItemsFilter)modable);
    }
}
