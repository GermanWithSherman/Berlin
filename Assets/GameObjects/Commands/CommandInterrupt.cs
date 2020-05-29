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

    public override IModable copyDeep()
    {
        var result = new CommandInterrupt();

        result.Keywords = Modable.copyDeep(Keywords);
        result.Keyword = Modable.copyDeep(Keyword);

        return result;
    }

    private void mod(CommandInterrupt original, CommandInterrupt mod)
    {
        Keywords = Modable.mod(original.Keywords, mod.Keywords);
        Keyword = Modable.mod(original.Keyword, mod.Keyword);


    }

    public void mod(CommandInterrupt modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandInterrupt modCommand = modable as CommandInterrupt;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
