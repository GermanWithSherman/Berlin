using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGotoLocation : Command
{
    public string LocationID = "";

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
            GameManager.Instance.locationGoto(Location);
        else
            GameManager.Instance.locationGoto(LocationID);
    }

    public static CommandsCollection GotoCommandsList(LocationConnection locationConnection)
    {
        var result = new CommandsCollection();
        if (locationConnection.interruptible)
        {
            /*Command interruptCommand = new Command();
            interruptCommand.type = Command.Type.Interrupt;
            interruptCommand.p["keywords"] = new string[] { locationConnection.Type };
            Add("interrupt", interruptCommand);*/
            result.Add(new CommandInterrupt(locationConnection.Type));
        }

        /*Command timePassCommand = new Command();
        timePassCommand.type = Command.Type.TimePass;
        timePassCommand.p["v"] = locationConnection.Duration;
        Add("timePass", timePassCommand);*/
        result.Add(new CommandTimePass(locationConnection.Duration.GetValueOrDefault(0)));

        /*Command locationCommand = new Command();
        locationCommand.type = Command.Type.GotoLocation;
        locationCommand.p["location"] = locationConnection.TargetLocation;
        Add("location",locationCommand);*/
        result.Add(new CommandGotoLocation(locationConnection.TargetLocation));


        return result;
    }
}
