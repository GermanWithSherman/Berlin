using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandsCollection : Dictionary<string, Command>
{
    public void execute()
    {
        foreach (Command c in Values)
        {
            c.execute();
        }
    }
}
