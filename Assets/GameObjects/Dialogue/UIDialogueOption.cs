using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogueOption : MonoBehaviour
{

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
    }

}
