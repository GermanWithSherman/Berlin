using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreenText : MonoBehaviour
{
    public string Format;

    public float Multiplier;

    public string Key;
    public TMPro.TextMeshProUGUI Text;

    public void UpdateCharacter(NPC npc)
    {
        dynamic v = npc[Key];

        if (Multiplier != 0)
            v *= Multiplier;

        if (String.IsNullOrWhiteSpace(Format))
            Text.text = Convert.ToString(v);
        else
            Text.text = String.Format(Format, v);
    }
}
