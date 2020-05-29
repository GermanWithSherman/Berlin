using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDebug : Command
{

    public CText Message = new CText();

    public override void execute(Data data)
    {
        Debug.Log(Message.Text());
    }

    public override IModable copyDeep()
    {
        var result = new CommandDebug();

        result.Message = Modable.copyDeep(Message);

        return result;
    }

    private void mod(CommandDebug original, CommandDebug mod) {
        Message = Modable.mod(original.Message, mod.Message);
    }

    public void mod(CommandDebug modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandDebug modCommand = modable as CommandDebug;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
