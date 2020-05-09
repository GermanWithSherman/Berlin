using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPause : Command
{
    public override void execute(Data data)
    {
        Command.pauseActive = true;
        Command.pausedCommands = new CommandsCollection();
    }
}
