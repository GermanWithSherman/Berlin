using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNameDialog : Dialog<SelectNameDialogSetting>
{

    public TMPro.TMP_InputField inputField;
    //public Button RandomButton;

    public void nameRandomize()
    {
        inputField.text = GameManager.Instance.WeightedStringListCache[_settings.ListID].value();
    }

    public override void setSettings(DialogSetting settings)
    {
        _settings = (SelectNameDialogSetting)settings;

        inputField.interactable = _settings.FreeChoice;
    }

    public void onSubmitClick()
    {
        _data["name"] = inputField.text;
        submit();
    }

}

public class SelectNameDialogSetting : DialogSetting {
    public string ListID;
    public bool FreeChoice = true;
}