using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NPCsCollection : Dictionary<string,string>
{
    private SortedDictionary<string, NPC> _cache = new SortedDictionary<string, NPC>();

    public new NPC this[string key]
    {
        get
        {
            NPC result;
            if (_cache.TryGetValue(key, out result))
                return result;

            result = GameManager.Instance.NPCsLibrary[key];
            _cache[key] = result;

            return result;
        }
        set
        {
            _cache[key] = value;
            if (this.ContainsKey(key))
                this.Remove(key);
            this.Add(key,value.id);
        }
    }

    public bool TryGetNPC(string key, out NPC result)
    {
        result = this[key];
        if (result == null)
            return false;
        return true;
    }
}

