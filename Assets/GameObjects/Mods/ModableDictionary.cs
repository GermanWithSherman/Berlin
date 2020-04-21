using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModableDictionary<V> : Dictionary<string,V>, IModable
{
    //public void mod(Dictionary<string,V> modData)
    public void mod(IModable modable)
    {
        if(modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        ModableDictionary<V> modData = (ModableDictionary<V>)modable;

        foreach (KeyValuePair<string,V> modkv in modData)
        {
            if (!ContainsKey(modkv.Key))
            {
                this[modkv.Key] = modkv.Value;
                continue;
            }

            if(this[modkv.Key].GetType() != modkv.Value.GetType()){
                Debug.LogError("Type mismatch");
                continue;
            }

            if(this[modkv.Key] is IModable)
            {
                ((IModable)this[modkv.Key]).mod((IModable)modkv.Value);
                continue;
            }

            this[modkv.Key] = modkv.Value;

        }
    }
}
