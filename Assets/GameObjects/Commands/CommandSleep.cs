using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSleep : Command
{
    public string AlarmActivatedRef;
    public string AlarmTimeRef;
    public int Duration = -1;
    public float MaxSleepFactor = 1.2f;


    public override void execute(Data data)
    {
        int duration = Duration;

        

        int currentSleepStat;

        if (data is GameData)
            currentSleepStat = ((GameData)data).CharacterData.PC.statSleep;
        else
            currentSleepStat = data["PC.statSleep"];

        Activity sleepActivity = GameManager.Instance.ActivityLibrary["sleep"];

        //https://stackoverflow.com/questions/17944/how-to-round-up-the-result-of-integer-division
        int requiredSleepSeconds = (1000000 - currentSleepStat - 1) / sleepActivity.statSleep + 1;
        int maximumSleepSeconds = Mathf.CeilToInt(requiredSleepSeconds * MaxSleepFactor);

        if (!String.IsNullOrEmpty(AlarmTimeRef))
        {
            int alarmTime = data[AlarmTimeRef];
            duration = GameManager.Instance.timeSecondsTils(alarmTime);

            if (!String.IsNullOrEmpty(AlarmActivatedRef))
            {
                bool alarmActivated = data[AlarmActivatedRef];
                if (!alarmActivated)
                    duration = -1;
            }


        }

        if (duration > maximumSleepSeconds)
            duration = UnityEngine.Random.Range(requiredSleepSeconds, maximumSleepSeconds);

        if (duration < 0)
            duration = requiredSleepSeconds;//= 3600; // TODO: calculate required sleep

        CommandsCollection sleepCommands = SleepCommandList(duration);
        sleepCommands.execute();
        return;
    }

    public static CommandsCollection SleepCommandList(int duration)
    {
        CommandsCollection result = new CommandsCollection();

        int duration25 = duration / 4;
        int duration75 = duration * 3 / 4;

        int middleDuration = UnityEngine.Random.Range(duration25, duration75);

        result.Add(new CommandInterrupt("SleepStart"));

        result.Add(new CommandTimePass(middleDuration, "sleep"));
        result.Add(new CommandInterrupt("SleepMiddle"));
        result.Add(new CommandTimePass(duration - middleDuration, "sleep"));

        result.Add(new CommandInterrupt("SleepEnd"));

        return result;
    }
}
