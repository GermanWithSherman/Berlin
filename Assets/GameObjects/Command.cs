using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public static bool breakActive = false;
    public static bool pauseActive = false;
    public static CommandsCollection pausedCommands = new CommandsCollection();

    public enum Type { None, Break, Pause, Continue, Flush, Consume, Sleep, Dialog, Event, EventEnd, GotoLocation, Interrupt, Outfit, Services, Set, Shop, TimePass }

    [JsonConverter(typeof(StringEnumConverter))]
    public Type type = Type.None;

    public Dictionary<string, dynamic> p = new Dictionary<string, dynamic>();   

    public void execute()
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.uiUpdate();

        switch (type)
        {
            case Type.None:
                return;
            case Type.Break:
                breakActive = true;
                return;
            case Type.Consume:

                int hunger = p.ContainsKey("hunger") ? ((int)p["hunger"]) : 0;
                int calories = p.ContainsKey("calories") ? ((int)p["calories"]) : 0;
                
                gameManager.PC.statHunger += hunger;
                gameManager.PC.statCalories += (int)calories;

                return;
            case Type.Continue:
                pauseActive = false;
                pausedCommands.execute();
                return;
            case Type.Dialog:

                string dialogId = p["id"];
                JObject targets = p.ContainsKey("targets") ? (p["targets"] is JObject ? p["targets"] : null) : null;
                JObject onComplete = p.ContainsKey("onComplete") ? (p["onComplete"] is JObject ? p["onComplete"] : null) : null;

                DialogResolver dialogResolver = new DialogResolver(targets);
                dialogResolver.setOnComplete(onComplete);

                gameManager.DialogServer.dialogShow(dialogId, dialogResolver);

                return;
            case Type.Event:

                if (p.ContainsKey("e"))
                {
                    string key = p["e"];
                    gameManager.eventExecute(key);
                }
                else
                {
                    string eventGroup;
                    string eventStage;
                    eventGroup = p["eg"];
                    eventStage = p["es"];
                    gameManager.eventExecute(eventGroup, eventStage);
                }

                return;
            case Type.EventEnd:
                gameManager.eventEnd();
                return;
            case Type.Flush:
                pauseActive = false;
                pausedCommands = new CommandsCollection();
                return;
            case Type.GotoLocation:
                if (p.ContainsKey("location") && p["location"] is SubLocation)
                {
                    gameManager.locationGoto(p["location"]);
                    return;
                }


                dynamic lid;

                if (!p.TryGetValue("id", out lid))
                {
                    Debug.LogError("Command malformed: parameter id missing");
                    return;
                }

                if (!(lid is System.String))
                {
                    Debug.LogError("Command malformed: parameter id must be a string");
                    return;
                }

                gameManager.locationGoto(lid);

                return;
            case Type.Interrupt:

                if (!p.ContainsKey("keywords"))
                {
                    Debug.LogError($"Parameter keywords missing in Command.Type.Interrupt");
                    return;
                }

                if (p["keywords"] is JArray) {
                    JArray interruptKeywords = p["keywords"];
                    List<string> keywords = interruptKeywords.ToObject<List<string>>();
                    gameManager.InterruptServer.trigger(keywords);

                    return;
                }

                if (p["keywords"] is IEnumerable<string>)
                {
                    gameManager.InterruptServer.trigger(p["keywords"]);
                    return;
                }

                if(p["keywords"] is string)
                {
                    gameManager.InterruptServer.trigger(p["keywords"]);
                    return;
                }


                Debug.LogError($"Parameter keywords malformed in Command.Type.Interrupt");
                 

                return;
            case Type.Outfit:
                gameManager.outfitWindowShow();
                return;
            case Type.Pause:
                pauseActive = true;
                pausedCommands = new CommandsCollection();
                return;
            case Type.Services:
                dynamic servicePointId;

                if (!p.TryGetValue("id", out servicePointId))
                {
                    Debug.LogError("Command malformed: parameter id missing");
                    return;
                }

                if (!(servicePointId is System.String))
                {
                    Debug.LogError("Command malformed: parameter id must be a string");
                    return;
                }

                gameManager.servicepointShow(servicePointId);


                return;
            case Type.Set:
                string k = p["k"];
                dynamic v = p["v"];

                gameManager.GameData[k] = v;

                return;
            case Type.Shop:
                string shopId = p["id"];
                gameManager.shopShow(shopId);
                return;
            case Type.Sleep:
                int duration = -1;
                if (p.ContainsKey("duration"))
                    duration = (int)p["duration"];
                else if (p.ContainsKey("alarmTime")) {
                    int alarmTime = GameManager.Instance.GameData[p["alarmTime"]];
                    duration = GameManager.Instance.timeSecondsTils(alarmTime);

                    if (p.ContainsKey("alarmActivated"))
                    {
                        bool alarmActivated = GameManager.Instance.GameData[p["alarmActivated"]];
                        if (!alarmActivated)
                            duration = -1;
                    }

                    
                }

                if (duration < 0)
                    duration = 3600; // TODO: calculate required sleep
                
                CommandsCollection sleepCommands = newSleepCommandList(duration);
                sleepCommands.execute();
                return;
            case Type.TimePass:
                Int64 time = p["v"];
                string activityId = p.ContainsKey("a") ? (string)p["a"] : "";
                gameManager.timePass((int)time,activityId);
                return;
        }

        
    }

    private static Command newInterruptCommand(string keyword)
    {
        return newInterruptCommand(new string[] { keyword });
    }

    private static Command newInterruptCommand(IEnumerable<string> keywords)
    {
        Command result = new Command();
        result.type = Type.Interrupt;
        result.p["keywords"] = keywords;
        return result;
    }

    private static CommandsCollection newSleepCommandList(int duration)
    {
        CommandsCollection result = new CommandsCollection();

        int duration25 = duration / 4;
        int duration75 = duration * 3 / 4;

        int middleDuration = UnityEngine.Random.Range(duration25,duration75);

        result.Add(newInterruptCommand("SleepStart"));

        result.Add(newTimePassCommand(middleDuration,"sleep"));
        result.Add(newInterruptCommand("SleepMiddle"));
        result.Add(newTimePassCommand(duration-middleDuration, "sleep"));

        result.Add(newInterruptCommand("SleepEnd"));

        return result;
    }

    private static Command newTimePassCommand(int duration, string activityId)
    {
        Command result = new Command();
        result.type = Type.TimePass;
        result.p["a"] = activityId;
        result.p["v"] = duration;
        return result;
    }
}
