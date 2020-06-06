using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IModableDictionary<V> : IDictionary<string, V>, IModable {

}


public class ModableValueTypeSortedDictionary<V> : SortedDictionary<string, V>, IModable, IModableDictionary<V>
{
    public IModable copyDeep()
    {
        return copyDeep(this);
    }

    public static T copyDeep<T>(T original) where T : ModableValueTypeSortedDictionary<V>, new()
    {
        var result = new T();
        foreach (KeyValuePair<string, V> entry in original)
        {
            result[entry.Key] = entry.Value;
        }
        return result;
    }


    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError($"Type mismatch in mod(): {modable.GetType()} != {GetType()}");
            return;
        }

        ModableValueTypeSortedDictionary<V> modData = (ModableValueTypeSortedDictionary<V>)modable;

        foreach (KeyValuePair<string, V> entry in modData)
        {

            this[entry.Key] = entry.Value;
        }

    }
}

public class ModableObjectSortedDictionary<V> : SortedDictionary<string, V>, IModable, IModableDictionary<V> where V : IModable
{

    public IModable copyDeep()
    {
        return copyDeep(this);
    }

    public static T copyDeep<T>(T original) where T : ModableObjectSortedDictionary<V>, new()
    {
        var result = new T();
        foreach (KeyValuePair<string, V> entry in original)
        {
            result[entry.Key] = Modable.copyDeep(entry.Value);
        }
        return result;
    }


    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError($"Type mismatch in mod(): {modable.GetType()} != {GetType()}");
            return;
        }

        ModableObjectSortedDictionary<V> modData = (ModableObjectSortedDictionary<V>)modable;

        foreach (KeyValuePair<string, V> entry in modData)
        {
            if (!ContainsKey(entry.Key))
            {
                this[entry.Key] = Modable.copyDeep(entry.Value);
                continue;
            }

            this[entry.Key] = Modable.mod(this[entry.Key], entry.Value);
        }

    }
}

public class ModableValueTypeHashDictionary<V> : Dictionary<string, V>, IModable, IModableDictionary<V>
{
    public IModable copyDeep()
    {
        return copyDeep(this);
    }

    public static T copyDeep<T>(T original) where T : ModableValueTypeHashDictionary<V>, new()
    {
        var result = new T();
        foreach (KeyValuePair<string, V> entry in original)
        {
            result[entry.Key] = entry.Value;
        }
        return result;
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError($"Type mismatch in mod(): {modable.GetType()} != {GetType()}");
            return;
        }

        ModableValueTypeHashDictionary<V> modData = (ModableValueTypeHashDictionary<V>)modable;

        foreach (KeyValuePair<string, V> entry in modData)
        {

            this[entry.Key] = entry.Value;
        }

    }
}

public class ModableObjectHashDictionary<V> : Dictionary<string, V>, IModable, IModableDictionary<V> where V : IModable
{

    public IModable copyDeep()
    {
        return copyDeep(this);
    }

    public static T copyDeep<T>(T original) where T : ModableObjectHashDictionary<V>, new()
    {
        var result = new T();
        foreach (KeyValuePair<string, V> entry in original)
        {
            result[entry.Key] = Modable.copyDeep(entry.Value);
        }
        return result;
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError($"Type mismatch in mod(): {modable.GetType()} != {GetType()}");
            return;
        }

        ModableObjectHashDictionary<V> modData = (ModableObjectHashDictionary<V>)modable;

        foreach (KeyValuePair<string, V> entry in modData)
        {
            if (!ContainsKey(entry.Key))
            {
                this[entry.Key] = Modable.copyDeep(entry.Value);
                continue;
            }

            this[entry.Key] = Modable.mod(this[entry.Key], entry.Value);
        }

    }


    /*public IModable copyDeep()
    {
        return copyDeep<ModableDictionary<V>>(this);
    }



    public static T copyDeep<T>(T original) where T : IModableDictionary<V>, new()
    {
        var result = new T();
        foreach (KeyValuePair<string, V> kv in original)
        {
            string key = kv.Key;
            V value = kv.Value;
            if (typeof(V) is IModable)
                result[key] = (V)Modable.copyDeep((IModable)value);
            else
                result[key] = value;
        }
        return result;
    }


    /*public void mod(IModable modable)
    {
        if(modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        ModableDictionary<V> modData = (ModableDictionary<V>)modable;

        if (typeof(V) is IModable)
        {
            foreach (KeyValuePair<string, V> modkv in modData)
            {
                if (!ContainsKey(modkv.Key))
                {
                    this[modkv.Key] = (V)Modable.copyDeep((IModable)modkv.Value);
                    continue;
                }

                this[modkv.Key] = (V)Modable.mod((IModable)this[modkv.Key], (IModable)modkv.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<string, V> modkv in modData)
            {
                this[modkv.Key] = modkv.Value;
            }
        }
    }*/

    /*public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod<V>(this,(ModableDictionary<V>)modable);
    }

    public static void mod<T>(IModableDictionary<T> original, IModableDictionary<T> mod) where T : IModable
    {
        throw new System.Exception("HIT");
    }

    public static void mod<T>(IModableDictionary<T> original, IModableDictionary<T> mod)
    {
        throw new System.Exception("HIT2");
    }

    public static void mod(IModable original, IModable modable)
    {
        if (modable.GetType() != original.GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }


        IModableDictionary<V> modData = (IModableDictionary<V>)modable;
        IModableDictionary<V> originalData = (IModableDictionary<V>)original;


        //if (typeof(V) is IModable)
        if(typeof(IModable).GetTypeInfo().IsAssignableFrom(typeof(V).Ge‌​tTypeInfo()))
        {
            foreach (KeyValuePair<string, V> modkv in modData)
            {
                if (!originalData.ContainsKey(modkv.Key))
                {
                    originalData[modkv.Key] = (V)Modable.copyDeep((IModable)modkv.Value);
                    continue;
                }

                originalData[modkv.Key] = (V)Modable.mod((IModable)originalData[modkv.Key], (IModable)modkv.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<string, V> modkv in modData)
            {
                originalData[modkv.Key] = modkv.Value;
            }
        }
    }

}

public class ModableSortedDictionary<V> : SortedDictionary<string,V>, IModable, IModableDictionary<V>
{
    public IModable copyDeep()
    {
        return ModableDictionary<V>.copyDeep<ModableSortedDictionary<V>>(this);
    }

    public static T copyDeep<T>(T original) where T : IModableDictionary<V>, new()
    {
        return ModableDictionary<V>.copyDeep<T>(original);
    }

    public void mod(IModable modable)
    {
        ModableDictionary<V>.mod(this, modable);
    }
}
´*/
}