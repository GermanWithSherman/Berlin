using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueOption : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI Text;

    private DialogueOption _dialogueOption;
    private UIDialogue _uIDialogue;

    public void onClick()
    {
        _uIDialogue.optionExecute(_dialogueOption);
    }

    public void setDialogueWindow(UIDialogue uIDialogue)
    {
        _uIDialogue = uIDialogue;
    }

    public void setDialogueOption(DialogueOption dialogueOption)
    {
        _dialogueOption = dialogueOption;
        Text.text = dialogueOption.Text;

        if (dialogueOption.Visible == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (dialogueOption.Enabled == false)
        {
            Text.color = Color.red;
            Text.text = $"{dialogueOption.Text} ({dialogueOption.Text})";
            Button.interactable = false;
        }
        else //true or null
        {
            Text.color = Color.black;
            Text.text = dialogueOption.Text;
            Button.interactable = true;
        }
    }

}
