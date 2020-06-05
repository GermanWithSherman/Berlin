using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModsMenuMod : MonoBehaviour
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDescription;

    public TextMeshProUGUI TextIsActive;

    public bool Activated = true;

    private Mod _mod;

    public void SetMod(Mod mod)
    {
        _mod = mod;

        TextName.text = _mod.DisplayName;
        TextDescription.text = $"Version {_mod.Version}";


    }

}
