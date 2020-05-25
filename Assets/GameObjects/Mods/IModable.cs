﻿using System;
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
        try
        {
            if (original == null)
                return default(T);
            return (T)original.copyDeep();
        }
        catch(InvalidCastException)
        {
            Debug.LogError($"{typeof(T)} vs {original.copyDeep().GetType()}");
            return default;
        }
    }

    public static DateTime? copyDeep(DateTime? original)
    {
        DateTime? result = original;
        return result;
    }

    public static bool copyDeep(bool original)
    {
        return original;
    }

    public static int copyDeep(int original)
    {
        return original;
    }

    public static int? copyDeep(int? original)
    {
        return original;
    }

    public static string copyDeep(string original)
    {
        return original;
    }

    public static T mod<T>(T original, T mod) where T : IModable
    {
        if (mod == null)
        {
            if (original == null)
                return default;
            return (T)original.copyDeep();
        }


        if (original == null)
            return (T)mod.copyDeep();

        T originalCopy = (T)original.copyDeep();
        originalCopy.mod(mod);

        return originalCopy;
    }

    public static DateTime? mod(DateTime? original, DateTime? mod)
    {
        if (mod == null)
            return original;
        return mod;
    }

    public static bool mod(bool original, bool mod)
    {
        return mod;
    }

    public static int mod(int original, int mod)
    {
        return mod;
    }

    public static int? mod(int? original, int? mod)
    {
        if (mod == null)
            return original;
        return mod;
    }

    public static string mod(string original, string mod)
    {
        if (String.IsNullOrEmpty(mod))
            return original;
        return mod;
    }
}