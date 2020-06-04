using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGotoLocation : Command
{
    public string LocationID = "";
    public bool SkipOnShow;

    [JsonIgnore]
    public SubLocation Location;

    public CommandGotoLocation()
    {
    }

    public CommandGotoLocation(string locationID)
    {
        LocationID = locationID;
    }

    public CommandGotoLocation(SubLocation location)
    {
        Location = location;
    }

    public override void execute(Data data)
    {
        if (Location != null)
            GameManager.Instance.locationGoto(Location, SkipOnShow);
        else
            GameManager.Instance.locationGoto(LocationID, SkipOnShow);
    }

    public static CommandsCollection GotoCommandsList(LocationConnection locationConnection)
    {
        var result = new CommandsCollection();
        if (locationConnection.interruptible)
        {
            result.Add(new CommandInterrupt(locationConnection.Type));
        }
        result.Add(new CommandTimePass(locationConnection.Duration.GetValueOrDefault(0)));
        result.Add(new CommandGotoLocation(locationConnection.TargetLocation));


        return result;
    }

    public override IModable copyDeep()
    {
        var result = new CommandGotoLocation();

        result.LocationID = Modable.copyDeep(LocationID);
        result.SkipOnShow = Modable.copyDeep(SkipOnShow);

        return result;
    }

    private void mod(CommandGotoLocation original, CommandGotoLocation mod)
    {
        LocationID = Modable.mod(original.LocationID, mod.LocationID);
        SkipOnShow = Modable.mod(original.SkipOnShow, mod.SkipOnShow);
    }

    public void mod(CommandGotoLocation modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandGotoLocation modCommand = modable as CommandGotoLocation;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
