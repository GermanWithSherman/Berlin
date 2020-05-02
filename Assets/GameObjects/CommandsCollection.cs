using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCollection : ModableDictionary<Command>
{

    public CommandsCollection() { }

    public CommandsCollection(LocationConnection locationConnection)
    {
        if (locationConnection.interruptible)
        {
            Command interruptCommand = new Command();
            interruptCommand.type = Command.Type.Interrupt;
            interruptCommand.p["keywords"] = new string[] { locationConnection.Type };
            Add("interrupt", interruptCommand);
        }

        Command timePassCommand = new Command();
        timePassCommand.type = Command.Type.TimePass;
        timePassCommand.p["v"] = locationConnection.Duration;
        Add("timePass", timePassCommand);

        Command locationCommand = new Command();
        locationCommand.type = Command.Type.GotoLocation;
        locationCommand.p["location"] = locationConnection.TargetLocation;
        Add("location",locationCommand);
    }

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
            if (Command.pauseActive && c.type != Command.Type.Continue && c.type != Command.Type.Flush)
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
