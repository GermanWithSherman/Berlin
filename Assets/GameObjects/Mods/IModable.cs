using System;
using System.Collections;
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


        if (original == null)
            return (T)mod.copyDeep();


        original.mod(mod);

        return original;
    }

    public static string mod(string original, string mod)
    {
        if (String.IsNullOrEmpty(mod))
            return original;
        return mod;
    }
}