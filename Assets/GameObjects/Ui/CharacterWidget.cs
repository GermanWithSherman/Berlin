using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWidget : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextName;

    public RawImage Image;

    public CharacterWidgetStat HungerStat;
    public CharacterWidgetStat HygieneStat;
    public CharacterWidgetStat SleepStat;

    public void show()
    {
        PC pc = GameManager.Instance.PC;

        TextName.text = GameManager.Instance.FunctionsLibrary.npcName(pc);

        Image.texture = pc.Texture;

        HygieneStat.setPercentage(pc.statHygiene);
        HungerStat.setPercentage(pc.statHunger);
        SleepStat.setPercentage(pc.statSleep);

        gameObject.SetActive(true);
    }

    public void visibilityToggle()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            show();
    }
}
