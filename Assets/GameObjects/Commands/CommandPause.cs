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

    public override IModable copyDeep()
    {
        return new CommandPause();
    }

    public override void mod(IModable modable) { }
}
