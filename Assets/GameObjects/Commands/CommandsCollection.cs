using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCollection : ModableObjectSortedDictionary<Command>, IModable
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
        execute(GameManager.Instance.GameData);
    }
    public void execute(Data data)
    {
        try
        {
            Command.breakActive = false;

            foreach (Command c in Values)
            {
                if (Command.pauseActive && !(c is CommandContinue) && !(c is CommandFlush))
                {
                    Command.pausedCommands.Add(c);
                    continue;
                }

                c.execute(data);
                if (Command.breakActive)
                    break;

            }
        }
        catch (GameException e)
        {
            ErrorMessage.Show(e.Message);
            Debug.LogError(e);
        }
        catch (Exception e)
        {
            ErrorMessage.Show(e.Message);
            Debug.LogError(e);
        }
    }

    public new IModable copyDeep()
    {
        return copyDeep(this);
    }
}
