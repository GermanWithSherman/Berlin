using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YesNoDialog : Dialog<YesNoDialogSettings>
{

    public TextMeshProUGUI Text;
    public TextMeshProUGUI Title;

    public void onButtonPressed(string button)
    {
        switch (button)
        {
            case ("Yes"):
                hide();
                _settings.onYes();
                return;
            case ("No"):
                hide();
                _settings.onNo();
                return;
        }
    }

    public override void setSettings(DialogSetting settings)
    {
        _settings = (YesNoDialogSettings)settings;
        Text.text = _settings.Text;
        Title.text = _settings.Title;
    }
}


public class YesNoDialogSettings : DialogSetting
{
    public delegate void Handler();

    public Handler onYes;
    public Handler onNo;

    public string Text;
    public string Title;
}