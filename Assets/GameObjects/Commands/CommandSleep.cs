using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSleep : Command
{
    public string AlarmActivatedRef;
    public string AlarmTimeRef;
    public int Duration = -1;


    public override void execute(Data data)
    {
        int duration = Duration;
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

        if (duration < 0)
            duration = 3600; // TODO: calculate required sleep

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
