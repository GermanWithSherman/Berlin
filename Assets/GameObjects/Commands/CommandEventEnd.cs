using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEventEnd : Command
{
    public override void execute(Data data)
    {
        GameManager.Instance.eventEnd();
    }
}
