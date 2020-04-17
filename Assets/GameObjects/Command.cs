﻿using Newtonsoft.Json;
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

    public enum Type { None, Break, Pause, Continue, Consume, Dialog, Event, EventEnd, GotoLocation, Interrupt, Outfit, Services, Set, Shop, TimePass }

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

                long hunger = p.ContainsKey("hunger") ? ( p["hunger"] is Int64 ? p["hunger"] : 0L ) : 0L;
                long calories=p.ContainsKey("calories") ? (p["calories"] is Int64 ? p["calories"] : 0L) : 0L;

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
            case Type.GotoLocation:
                dynamic lid;

                if (!p.TryGetValue("id", out lid))
                {
                    Debug.LogError("Command malformed: parameter id missing");
                    return;
                }

                if(!(lid is System.String))
                {
                    Debug.LogError("Command malformed: parameter id must be a string");
                    return;
                }

                gameManager.locationGoto(lid);
                
                return;
            case Type.Interrupt:
                JArray interruptKeywords = p.ContainsKey("keywords") ? (p["keywords"] is JArray ? p["keywords"] : null) : null;
                
                if(interruptKeywords == null)
                {
                    Debug.LogError($"Interrupt Parameter keywords malformed");
                    return;
                }

                List<string> keywords = interruptKeywords.ToObject<List<string>>();

                gameManager.InterruptServer.trigger(keywords);

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
            case Type.TimePass:
                Int64 time = p["v"];
                gameManager.timePass((int)time);
                return;
        }

        
    }
}
