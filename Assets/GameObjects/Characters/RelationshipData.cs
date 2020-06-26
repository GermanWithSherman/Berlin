using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RelationshipData : ModableValueTypeHashDictionary<string>, IModable
{
    public string GetRelationshipTo(NPC to)
    {
        string key = to.id;
        if (ContainsKey(key))
            return this[key];
        return "";
    }

    public new IModable copyDeep()
    {
        return copyDeep(this);
    }
}

