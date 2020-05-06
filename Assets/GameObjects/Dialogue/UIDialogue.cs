using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogue : MonoBehaviour
{
    private NPC _npc;

    private DialogueTopic _currentTopic;

    public TextMeshProUGUI Text;

    public UIDialogueOption OptionPrefab;
    public UIDialogueTopic TopicPrefab;

    public Transform OptionListTransform;
    public Transform TopicListTransform;



    
    void Start()
    {
        Text.text = "";
    }

    public void close()
    {
        hide();
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
        string t = line.Text.Text();
        Text.text += t + "\n\n";

        optionListShow(line.Options.Values);
    }

    public void optionExecute(DialogueOption option)
    {
        option.execute();

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

    

    public void show(NPC npc)
    {
        _npc = npc;

        List<DialogueTopic> dialogueTopics = GameManager.Instance.DialogueTopicLibrary.getTopicsByNPC(_npc);

        topicListShow(dialogueTopics);
        optionListClear();

        gameObject.SetActive(true);

        
    }

    public void stageShow(DialogueStage stage)
    {
        lineShow(stage.Line());
    }

    public void stageShow(string stageId)
    {
        DialogueStage stage = GameManager.Instance.DialogueLineCache.Stage(stageId,_currentTopic);
        stageShow(stage);
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

    public void topicShow(DialogueTopic topic)
    {
        _currentTopic = topic;
        DialogueStage stage = GameManager.Instance.DialogueLineCache[topic].StartStage();
        stageShow(stage);
    }

    
}
