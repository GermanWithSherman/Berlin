using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexSelectDialogPrefab : Dialog<SexSelectDialogSetting>
{
    public void genderSelect(string gender)
    {
        _data["Gender"] = gender;
        submit();
    }

    public override void setSettings(DialogSetting settings){
        _settings = (SexSelectDialogSetting)settings;
    }
}

public class SexSelectDialogSetting : DialogSetting{}
