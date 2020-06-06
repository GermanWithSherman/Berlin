using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Modable(ModableAttribute.FieldOptions.OptOut)]
public class CommandCall : Command, IModable//, IModableAutofields
{
    public string ProcedureID;
    public ModableListDynamic Parameters;

    public CommandCall() { }

    public override void execute(Data data)
    {
        GameManager.Instance.ProcedureExecute(ProcedureID,Parameters);
    }

    public override IModable copyDeep()
    {
        var result = new CommandCall();
        result.ProcedureID = Modable.copyDeep(ProcedureID);
        result.Parameters = Modable.copyDeep(Parameters);
        return result;
    }
    public override void mod(IModable modable) {
        var modCommand = (CommandCall)modable;
        ProcedureID = Modable.mod(ProcedureID, modCommand.ProcedureID);
        Parameters = Modable.mod(Parameters, modCommand.Parameters);
    }
        
    
}
