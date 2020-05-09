using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandContinue : Command
{
    public override void execute(Data data)
    {
        Command.pauseActive = false;
        Command.pausedCommands.execute();
    }
}
