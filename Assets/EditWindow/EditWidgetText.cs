using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWidgetText : EditWidget<string>
{
    public TMPro.TMP_InputField TextInputField;

    public override string getCurrentValue()
    {
        return TextInputField.text;
    }

    public override void setCurrentValue(string currentValue)
    {
        TextInputField.text = currentValue;
    }
}
