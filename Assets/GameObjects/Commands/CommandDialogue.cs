using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CommandDialogue : Command
{
    public string NPCID;
    public NPC NPC { get => GameManager.Instance.NPCsLibrary[NPCID]; }

    public string TopicID;
    public DialogueTopic Topic { get => GameManager.Instance.DialogueTopicLibrary[TopicID]; }

    public override void execute(Data data)
    {
        if (TopicID.StartsWith("CONTINUE>"))
            GameManager.Instance.dialogueContinue(TopicID.Substring(9));
        else
            GameManager.Instance.dialogueShow(NPC, Topic);
    }
}

