using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
    private NPC _npc;

    private DialogueTopic _currentTopic;

    //public TextMeshProUGUI Text;
    public TextExtended Text;
    public ScrollRect TextScrollRect;

    public UIDialogueOption OptionPrefab;
    public UIDialogueTopic TopicPrefab;

    public Transform OptionListTransform;
    public Transform TopicListTransform;

    public GameObject FinishButton;


    public Data Data; //Data used to display text
    

    public void close()
    {
        hide();
        GameManager.Instance.uiUpdate();
    }

    public void finish()
    {
        close();
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    

    public void lineShow(DialogueLine line)
    {
        line.onShow.execute();

        string t = line.Text(Data);

        textShow(t);

        if (line.TopicsVisible)
        {
            topicListShow(_npc);
            TopicListTransform.gameObject.SetActive(true);
        }
        else
            TopicListTransform.gameObject.SetActive(false);

        if (line.LeaveEnabled)
            FinishButton.SetActive(true);
        else
            FinishButton.SetActive(false);

        optionListShow(line.Options.Values);
    }

    private void textShow(string t)
    {
        if (String.IsNullOrEmpty(t))
            return;

        //t = lineParse(t) + "\n\n";
        t += "\n\n";

        //Text.Text += t;
        Text.addText(t);
        Canvas.ForceUpdateCanvases(); 
        TextScrollRect.verticalNormalizedPosition = 0f; //do ForceUpdateCanvases first to take the new text into account
    }

    public void optionExecute(DialogueOption option)
    {
        option.execute();

        textShow(option.Say);

        stageShow(option.TargetStage);
    }

    private void optionListClear()
    {
        foreach (Transform child in OptionListTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void optionListShow(IEnumerable<DialogueOption> options)
    {
        optionListClear();

        foreach (DialogueOption option in options)
        {
            UIDialogueOption uiOption = Instantiate(OptionPrefab, OptionListTransform);
            uiOption.setDialogueWindow(this);
            uiOption.setDialogueOption(option);
        }
    }

    private void setData(NPC npc)
    {
        Dictionary<string, Data> dict = new Dictionary<string, Data>();
        dict["_NPC1"] = npc;
        dict["_PC"] = GameManager.Instance.PC;

        Data = new DataComposed(dict);
    }

    public void show(NPC npc, DialogueTopic topic = null)
    {
        _npc = npc;

        setData(_npc);

        Text.Text = "";
        //List<DialogueTopic> dialogueTopics = GameManager.Instance.DialogueTopicLibrary.getTopicsByNPC(_npc);
        //topicListShow(dialogueTopics);
        topicListShow(_npc);
        optionListClear();

        if (topic == null)
        {
            DialogueTopic greetingTopic = GameManager.Instance.DialogueTopicLibrary.getGreetingTopicByNPC(_npc);
            if (greetingTopic != null)
                topicShow(greetingTopic);
        }
        else
        {
            topicShow(topic);
        }

        gameObject.SetActive(true);

        
    }

    public void stageShow(DialogueStage stage)
    {
        lineShow(stage.Line());
    }

    public void stageShow(string stageId)
    {
        try
        {
            DialogueStage stage = GameManager.Instance.DialogueLineCache.Stage(stageId, _currentTopic);
            stageShow(stage);
        }
        catch
        {
            textShow("ERROR: The requested Dialoguestage can't be found");
        }
    }

    private void topicListClear()
    {
        foreach(Transform child in TopicListTransform)
        {
            Destroy(child.gameObject);
        }
    }

    private void topicListShow(IEnumerable<DialogueTopic> topics)
    {
        topicListClear();

        foreach (DialogueTopic topic in topics)
        {
            UIDialogueTopic uiTopic = Instantiate(TopicPrefab, TopicListTransform);
            uiTopic.setDialogueWindow(this);
            uiTopic.setTopic(topic);
        }
    }

    private void topicListShow(NPC _npc)
    {
        List<DialogueTopic> dialogueTopics = GameManager.Instance.DialogueTopicLibrary.getTopicsByNPC(_npc);
        topicListShow(dialogueTopics);
    }
        

    public void topicShow(DialogueTopic topic)
    {
        try
        {
            _currentTopic = topic;
            DialogueStage stage = GameManager.Instance.DialogueLineCache[topic].StartStage();
            stageShow(stage);
        }
        catch
        {
            textShow("ERROR: The requested Dialoguestage can't be found");
        }
    }

    
}

