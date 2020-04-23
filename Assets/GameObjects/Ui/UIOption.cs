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

        if (optionState.visible == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (optionState.enabled == false)
        {
            t.color = Color.red;
            t.text = $"{option.Text} ({optionState.text})";
            button.interactable = false;
        }
        else //true or null
        {
            t.color = Color.black;
            t.text = option.Text;
            button.interactable = true;
        }

        

        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => option.execute());
    }
}
