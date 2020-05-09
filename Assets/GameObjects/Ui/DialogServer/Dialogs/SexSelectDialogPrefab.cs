using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexSelectDialogPrefab : Dialog<SexSelectDialogSetting>
{
    public void genderSelect(string gender)
    {
        data["gender"] = gender;
        submit();
    }

    public override void setSettings(DialogSetting settings){}
}

public class SexSelectDialogSetting : DialogSetting{}
