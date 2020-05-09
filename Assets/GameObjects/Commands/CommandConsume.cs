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
}
