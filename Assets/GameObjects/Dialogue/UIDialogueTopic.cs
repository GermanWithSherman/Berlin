using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogueTopic : MonoBehaviour
{
    private DialogueTopic _topic;
    private UIDialogue _dialogueWindow;

    public TextMeshProUGUI Text;

    public void onClick()
    {
        _dialogueWindow.topicShow(_topic);
    }

    public void setDialogueWindow(UIDialogue dialogueWindow)
    {
        _dialogueWindow = dialogueWindow;
    }

    public void setTopic(DialogueTopic topic)
    {
        _topic = topic;

        Text.text = _topic.Title.Text();
    }

}
