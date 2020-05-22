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
}
