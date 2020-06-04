using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class CommandCall : Command, IModableAutofields
{
    public string ProcedureID;
    public ModableListDynamic Parameters;

    

    public override void execute(Data data)
    {
        GameManager.Instance.ProcedureExecute(ProcedureID,Parameters);
    }

    public override IModable copyDeep() => null;
    public override void mod(IModable modable) { }
        
    
}
