using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandServices : Command
{
    public string ServicePointID;
    public override void execute(Data data)
    {
        GameManager.Instance.servicepointShow(ServicePointID);
    }

    public override IModable copyDeep()
    {
        var result = new CommandServices();

        result.ServicePointID = Modable.copyDeep(ServicePointID);

        return result;
    }

    private void mod(CommandServices original, CommandServices mod)
    {
        ServicePointID = Modable.mod(original.ServicePointID, mod.ServicePointID);


    }

    public void mod(CommandServices modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandServices modCommand = modable as CommandServices;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
