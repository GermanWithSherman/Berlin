using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInterrupt : Command
{
    public ModableStringList Keywords = new ModableStringList();
    public string Keyword;

    public CommandInterrupt()
    {
    }

    public CommandInterrupt(string keyword)
    {
        Keyword = keyword;
    }

    public CommandInterrupt(ModableStringList keywords)
    {
        Keywords = keywords;
    }

    public override void execute(Data data)
    {

        if (!System.String.IsNullOrEmpty(Keyword))
            Keywords.Add(Keyword);

        GameManager.Instance.InterruptServer.trigger(Keywords);

    }
}
