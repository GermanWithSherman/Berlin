using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCollection : Dictionary<string, Command>
{
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
            if (Command.pauseActive && c.type != Command.Type.Continue)
            {
                Command.pausedCommands.Add(c);
                continue;
            }

            c.execute();
            if (Command.breakActive)
                break;
            
        }
    }
}
