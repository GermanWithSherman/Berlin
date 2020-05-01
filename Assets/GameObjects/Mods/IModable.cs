﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModable
{
    void mod(IModable modable);
    IModable copyDeep();
}

public static class Modable
{
    public static T copyDeep<T>(T original) where T: IModable
    {
        if (original == null)
            return default(T);
        return (T)original.copyDeep();
    }

    public static T mod<T>(T original, T mod) where T : IModable
    {
        if (mod == null)
            return original;

        /*if (!(mod is Conditional<T>))
        {
            Debug.LogError("Type mismatch");
            return original;
        }*/

        if (original == null)
            return (T)mod.copyDeep();
            //return ((Conditional<T>)mod.copyDeep());

            /*if (original.GetType() != mod.GetType())
            {
                Debug.LogError("Type mismatch");
                return original;
            }*/

        original.mod(mod);

        return original;
    }
}