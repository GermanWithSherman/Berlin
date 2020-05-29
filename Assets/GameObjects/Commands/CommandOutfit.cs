using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandOutfit : Command
{
    public OutfitRequirement OutfitRequirement = new OutfitRequirement();
    public CommandsCollection onClose = new CommandsCollection();

    public override void execute(Data data)
    {
        
        GameManager.Instance.outfitWindowShow(OutfitRequirement, onClose);
    }

    public override IModable copyDeep()
    {
        var result = new CommandOutfit();

        result.OutfitRequirement = Modable.copyDeep(OutfitRequirement);
        result.onClose = Modable.copyDeep(onClose);

        return result;
    }

    private void mod(CommandOutfit original, CommandOutfit mod)
    {
        OutfitRequirement = Modable.mod(original.OutfitRequirement, mod.OutfitRequirement);
        onClose = Modable.mod(original.onClose, mod.onClose);


    }

    public void mod(CommandOutfit modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandOutfit modCommand = modable as CommandOutfit;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
