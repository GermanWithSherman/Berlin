using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConsume : Command
{
    public int Hunger = 0;
    public int Calories = 0;

    public override void execute(Data data)
    {
        GameManager.Instance.PC.statHunger += Hunger;
        GameManager.Instance.PC.statCalories += Calories;
    }

    public override IModable copyDeep()
    {
        var result = new CommandConsume();

        result.Hunger = Modable.copyDeep(Hunger);
        result.Calories = Modable.copyDeep(Calories);

        return result;
    }

    private void mod(CommandConsume original, CommandConsume mod)
    {
        Hunger = Modable.mod(original.Hunger, mod.Hunger);
        Calories = Modable.mod(original.Calories, mod.Calories);


    }

    public void mod(CommandConsume modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandConsume modCommand = modable as CommandConsume;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
