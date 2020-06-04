using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEventEnd : Command
{
    public override void execute(Data data)
    {
        GameManager.Instance.eventEnd();
    }

    public override IModable copyDeep()
    {
        return new CommandEventEnd();
    }

    public override void mod(IModable modable) { }
}
