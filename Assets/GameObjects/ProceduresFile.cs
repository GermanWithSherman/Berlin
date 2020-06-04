using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduresFile : ModableDictionary<CommandsCollection>, IModable
{
    public new IModable copyDeep()
    {
        return base.copyDeep<ProceduresFile>();
    }
}
