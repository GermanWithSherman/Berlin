using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModsMenuMod : MonoBehaviour
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDescription;

    public TextMeshProUGUI TextIsActive;

    private bool _activated = true;
    public bool Activated
    {
        get => _activated;
        set
        {
            if (_activated != value)
            {
                if(GameManager.Instance.ModsServer.ModStateSet(_mod, value))
                {
                    _activated = value;
                    UpdateTextColors();
                }
                
            }
        }
    }

    private Mod _mod;

    public void Toggle()
    {
        Activated = !Activated;
    }

    public void SetMod(Mod mod,bool activated)
    {
        _activated = activated;
        _mod = mod;

        TextName.text = _mod.DisplayName;
        TextDescription.text = $"Version {_mod.Version}";

        UpdateTextColors();
    }

    private void UpdateTextColors()
    {
        if (_activated)
        {
            TextName.color = Color.black;
            TextDescription.color = Color.black;
        }
        else
        {
            TextName.color = Color.grey;
            TextDescription.color = Color.grey;
        }
    }


}
