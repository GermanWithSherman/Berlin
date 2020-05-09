using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCollection : ModableDictionary<Command>, IModable
{

    public CommandsCollection() { }

    public void Add(Command command)
    {
        string key;
        int i = Count;

        do
        {
            key = "#_" + i.ToString();
            i++;
        } while (ContainsKey(key));

        this[key] = command;
    }

    public void execute()
    {
        Command.breakActive = false;

        foreach (Command c in Values)
        {
            if(Command.pauseActive && !(c is CommandContinue) && !(c is CommandFlush))
            {
                Command.pausedCommands.Add(c);
                continue;
            }

            c.execute();
            if (Command.breakActive)
                break;
            
        }
    }

    public new IModable copyDeep()
    {
        return base.copyDeep<CommandsCollection>();
    }
}
