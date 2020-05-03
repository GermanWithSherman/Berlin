using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogue : MonoBehaviour
{
    private NPC _npc;

    public UIDialogueTopic TopicPrefab;



    public void show(NPC npc)
    {
        _npc = npc;

        List<DialogueTopic> dialogueTopics = GameManager.Instance.DialogueTopicLibrary.getTopicsByNPC(_npc);

        dialogueTopics.ForEach(topic =>
        {
            Debug.Log(topic.ID);
        });


        gameObject.SetActive(true);

        
    }
}
