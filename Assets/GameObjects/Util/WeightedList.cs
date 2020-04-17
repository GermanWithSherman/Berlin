using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedList<T>
{
    private struct WeightedListItem<S>
    {
        internal int weight;
        internal S value;

    }

    private List<WeightedListItem<T>> items = new List<WeightedListItem<T>>();
    private int weightSum = 0;

    public static implicit operator T(WeightedList<T> weightedList)
    {
        return weightedList.value();
    }

    private void add(WeightedListItem<T> weightedListItem)
    {
        items.Add(weightedListItem);
        weightSum += weightedListItem.weight;
    }

    protected void add(T value, int weight)
    {
        WeightedListItem<T> weightedListItem = new WeightedListItem<T>();
        weightedListItem.value = value;
        weightedListItem.weight = weight;
        add(weightedListItem);
    }

    public T value()
    {
        if(items.Count == 0)
            return default;

        int r = UnityEngine.Random.Range(1,weightSum);

        foreach(WeightedListItem<T> item in items)
        {
            if (r <= item.weight)
                return item.value;
            r -= item.weight;
        }

        return default;

    }

}
