using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFlush : Command
{
    public override void execute(Data data)
    {
        Command.pauseActive = false;
        Command.pausedCommands = new CommandsCollection();
    }
}
