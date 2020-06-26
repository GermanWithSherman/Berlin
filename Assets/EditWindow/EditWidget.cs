using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditWidget<T> : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Label;


    public abstract T getCurrentValue();
    public void setLabel(string label)
    {
        Label.text = label;
    }
    public abstract void setCurrentValue(T currentValue);
}