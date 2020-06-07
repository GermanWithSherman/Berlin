using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IModableDictionary<V> : IDictionary<string, V>, IModable {

}


public class ModableValueTypeSortedDictionary<V> : SortedDictionary<string, V>, IModable, IModableDictionary<V>
{
    public ModableValueTypeSortedDictionary() : base(new ModableSortedDictionaryComparer()) { }

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
    public ModableObjectSortedDictionary():base(new ModableSortedDictionaryComparer()){}

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
    public ModableValueTypeHashDictionary() { }

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

    public ModableObjectHashDictionary() { }

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

    public override string ToString()
    {
        return base.ToString()+"\n"+ JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
    }
}

public class ModableSortedDictionaryComparer : Comparer<string>
{
    public override int Compare(string x, string y)
    {
        if (y.Length > x.Length && y.StartsWith(x) && y[x.Length] == '^')
            return 1;

        if (x.Length > y.Length && x.StartsWith(y) && x[y.Length] == '^')
            return -1;

        return Comparer<string>.Default.Compare(x,y);
        
    }
}