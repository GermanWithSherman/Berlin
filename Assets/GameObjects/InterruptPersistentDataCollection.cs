using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptPersistentDataCollection : Dictionary<string, InterruptPersistentData>
{
    
    private DateTime cooldownCalc(int cooldown)
    {
        DateTime now = GameManager.Instance.GameData.WorldData.DateTime;

        if (cooldown > 0)
        {
            DateTime result = now + new TimeSpan(0, 0, cooldown);
            return result;
        }

        throw new NotImplementedException();
    }
    
    public int remainingCooldown(Interrupt interrupt)
    {
        string key = interrupt.id;
        if (!ContainsKey(key))
            return 0;

        DateTime now = GameManager.Instance.GameData.WorldData.DateTime;

        TimeSpan timeSpan = this[key].CooldownTil - now;

        int totalSeconds = Mathf.Max((int)timeSpan.TotalSeconds,0);
        return totalSeconds;
    }

    public void resetCooldown(Interrupt interrupt, int cooldown)
    {
        if (cooldown == 0)
            return;
        string key = interrupt.id;
        if (!ContainsKey(key))
            this[key] = new InterruptPersistentData();
        this[key].CooldownTil = cooldownCalc(cooldown);
    }
}
