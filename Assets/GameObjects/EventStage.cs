using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventStage
{
    public string id;

    public string text;

    public List<Command> commands = new List<Command>();

    public Dictionary<string, Option> options = new Dictionary<string, Option>();

    public void execute()
    {
        GameManager gameManager = GameManager.Instance;

        if (!String.IsNullOrEmpty(text))
            gameManager.TextMain = text;

        gameManager.optionsSet(options.Values);

        foreach (Command command in commands)
        {
            command.execute();
        }
    }

}
