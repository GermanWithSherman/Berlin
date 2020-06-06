using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using TMPro;

using System.Collections.Generic;
using UnityEngine;

public class SelectHorizontal : Dialog<SelectHorizontalSettings>
{
    public ButtonTextExtended ButtonPrefab;

    public Transform ButtonTransform;

    public TextMeshProUGUI HeadlineText;

    private SelectHorizontalSettings _selectHorizontalSettings;

    public override void setSettings(DialogSetting settings)
    {
        _settings = (SelectHorizontalSettings)settings;

        ButtonTransform.childrenDestroyAll();

        if (String.IsNullOrEmpty(_settings.Headline))
            HeadlineText.gameObject.SetActive(false);
        else
        {
            HeadlineText.text = _settings.Headline;
            HeadlineText.gameObject.SetActive(true);
        }

        

        foreach (Option option in _settings.Options.Values)
        {
            if (option.Visible)
            {
                ButtonTextExtended button = Instantiate(ButtonPrefab, ButtonTransform);
                button.Enabled = option.Enabled;
                button.Text = option.Text;
                button.Button.onClick.AddListener(delegate { option.Commands.execute(_data); submit(); });
            }
        }
    }


}

public class SelectHorizontalSettings : DialogSetting
{
    public string Headline;
    public ModableObjectSortedDictionary<Option> Options = new ModableObjectSortedDictionary<Option>();
}

