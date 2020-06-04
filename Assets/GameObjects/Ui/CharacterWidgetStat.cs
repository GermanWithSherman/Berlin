using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterWidgetStat : MonoBehaviour
{
    public TextMeshProUGUI TextLabel;
    public TextMeshProUGUI TextPercantage;

    public Color Color
    {
        get => TextLabel.color;
        set
        {
            TextLabel.color = value;
            TextPercantage.color = value;
        }
    }

    public string Label;

    void Start()
    {
        TextLabel.text = Label;
    }

    public void setPercentage(int value)
    {
        Color color = Color.white;


        if (value >= 200000)
            color = Color.HSVToRGB((value - 200000) / 800000f * 140f / 360f, 1, 1);
        else
            color = Color.red;

        Color = color;
        TextPercantage.text = (value / 10000).ToString()+" %";
    }
}
