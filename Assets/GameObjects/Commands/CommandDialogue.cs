using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CommandDialogue : Command
{
    public string NPCID;
    [JsonIgnore]
    public NPC NPC { get => GameManager.Instance.NPCsLibrary[NPCID]; }

    public string TopicID;
    [JsonIgnore]
    public DialogueTopic Topic { get => GameManager.Instance.DialogueTopicLibrary[TopicID]; }

    public override void execute(Data data)
    {
        if (TopicID.StartsWith("CONTINUE>"))
            GameManager.Instance.dialogueContinue(TopicID.Substring(9));
        else
            GameManager.Instance.dialogueShow(NPC, Topic);
    }

    public override IModable copyDeep()
    {
        var result = new CommandDialogue();

        result.NPCID = Modable.copyDeep(NPCID);
        result.TopicID = Modable.copyDeep(TopicID);

        return result;
    }

    private void mod(CommandDialogue original, CommandDialogue mod)
    {
        NPCID = Modable.mod(original.NPCID, mod.NPCID);
        TopicID = Modable.mod(original.TopicID, mod.TopicID);


    }

    public void mod(CommandDialogue modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandDialogue modCommand = modable as CommandDialogue;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}

