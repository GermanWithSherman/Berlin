using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandBreak : Command
{
    public override void execute(Data data)
    {
        Command.breakActive = true;
    }

}
