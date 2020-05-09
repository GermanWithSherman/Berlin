using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClockDialogPrefab : Dialog<AlarmClockDialogSetting>
{

    public TMPro.TMP_InputField inputField;

    public void onSubmitClick()
    {
        string timeRaw = inputField.text;

        string[] timeParts = timeRaw.Split(':');

        int time = Int32.Parse(timeParts[0]) * 10000 + Int32.Parse(timeParts[1]) * 100 + Int32.Parse(timeParts[2]);

        //data["time"] = time;

        //submit();
    }

    public override void setSettings(DialogSetting settings){}
}

public class AlarmClockDialogSetting : DialogSetting { }