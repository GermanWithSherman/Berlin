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
    public TextExtended TextHistory;
    public TextExtended TextCurrent;
    public ScrollRect TextHistoryScrollRect;
    public ScrollRect TextCurrentScrollRect;

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

    public void contin(string stageID){
        show();

        string[] keyparts = stageID.Split('.');

        if (!String.IsNullOrWhiteSpace(keyparts[0]))
            _currentTopic = GameManager.Instance.DialogueTopicLibrary[keyparts[0]];

        DialogueStage ds = GameManager.Instance.DialogueLineCache.Stage(stageID, _currentTopic);
        stageShow(ds);
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
        TextHistory.addText(t);
        TextCurrent.Text = t;
        Canvas.ForceUpdateCanvases();
        TextCurrentScrollRect.verticalNormalizedPosition = 1f; //do ForceUpdateCanvases first to take the new text into account
        TextHistoryScrollRect.verticalNormalizedPosition = 0f; //do ForceUpdateCanvases first to take the new text into account
    }

    public void optionExecute(DialogueOption option)
    {
        option.execute();

        textShow(option.Say);

        if (option.TargetStage.StartsWith(">"))
        {
            switch (option.TargetStage.Substring(1))
            {
                case ("END"):
                    hide();
                    break;
            }
        }
        else
        {
            stageShow(option.TargetStage);
        }

        GameManager.Instance.uiUpdate();
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

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void show(DialogueTopic topic = null)
    {
        

        TextCurrent.Text = "";
        TextHistory.Text = "";

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


        show();


    }

    public void show(NPC npc, DialogueTopic topic = null)
    {
        _npc = npc;

        GameManager.Instance.GameData.NPCsActive["_NPC1"] = npc;

        setData(_npc);

        show(topic);
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
        catch(Exception e)
        {
            Debug.LogError(e);
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
        catch(Exception e)
        {
            textShow("ERROR: The requested Dialoguestage can't be found");
            Debug.LogError(e);
        }
    }

    
}

