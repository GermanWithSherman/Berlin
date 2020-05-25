using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOption : MonoBehaviour
{
    public void optionSet(Option option)
    {
        Option.OptionState optionState = option.State;


        Text t = GetComponentInChildren<Text>();
        Button button = GetComponentInChildren<Button>();

        if (optionState.Visible == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (optionState.Enabled == false)
        {
            t.color = Color.red;
            t.text = $"{option.Text} ({optionState.Text})";
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
        button.onClick.AddListener(() => GameManager.Instance.uiUpdate());
    }
}
