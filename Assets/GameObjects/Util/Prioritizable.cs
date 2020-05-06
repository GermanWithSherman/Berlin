using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPrioritizable
{
    int getPriority();
}

public static class Prioritizable
{
    private static int comparePriority<T>(T x, T y) where T : IPrioritizable
    {
        return y.getPriority() - x.getPriority();
    }

    public static void Sort<T>(List<T> list) where T : IPrioritizable
    {
        list.Sort(comparePriority);
    }
}
