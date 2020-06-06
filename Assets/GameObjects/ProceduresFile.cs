using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduresFile : ModableObjectHashDictionary<CommandsCollection>, IModable
{
    public new IModable copyDeep()
    {
        return copyDeep(this);
    }
}
