using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    public void optionSet(Option option)
    {
        Option.OptionState optionState = option.state;


        Text t = GetComponentInChildren<Text>();
        Button button = GetComponentInChildren<Button>();

        if (optionState.enabled)
        {
            t.color = Color.black;
            t.text = option.Text;
            button.interactable = true;
        }
        else
        {
            t.color = Color.red;
            t.text = $"{option.Text} ({optionState.text})";
            button.interactable = false;
        }

        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => option.execute());
    }
}
