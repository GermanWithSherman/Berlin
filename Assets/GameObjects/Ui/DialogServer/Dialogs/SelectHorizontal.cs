using Newtonsoft.Json.Linq;
using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class SelectHorizontal : Dialog<SelectHorizontalSettings>
{
    public ButtonTextExtended ButtonPrefab;

    private SelectHorizontalSettings _selectHorizontalSettings;

    public override void setSettings(DialogSetting settings)
    {
        transform.childrenDestroyAll();

        SelectHorizontalSettings _settings = (SelectHorizontalSettings)settings;

        foreach (Option option in _settings.Options.Values)
        {
            ButtonTextExtended button = Instantiate(ButtonPrefab, transform);
            button.Text = option.Text;
        }
    }

    /*public override void setSettings(IDictionary<string, string> settings)
    {
        transform.childrenDestroyAll();

        /*base.setSettings(settings);

        int optionCount = Int32.Parse(settings["optionCount"]);

        for(int i = 1; i <= optionCount; i++)
        {
            ButtonTextExtended button = Instantiate(ButtonPrefab, transform);
            button.Text = settings["text_"+i.ToString()];

            string result = settings["result_" + i.ToString()];
            button.Button.onClick.AddListener(delegate { optionSelect(result); });
        }*//*

    }*/


    /*public void optionSelect(string result)
    {
        data["RESULT"] = result;
        submit();
    }*/

}

public class SelectHorizontalSettings : DialogSetting
{
    public ModableDictionary<Option> Options = new ModableDictionary<Option>();
}

