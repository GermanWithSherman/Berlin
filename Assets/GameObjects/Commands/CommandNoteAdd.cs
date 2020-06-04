using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNoteAdd : Command
{
    public string NoteID;
    public CText Text;

    public override void execute(Data data)
    {
        string text = Text.Text(data);
        GameManager.Instance.noteAdd(NoteID,text);
    }

    public override IModable copyDeep()
    {
        var result = new CommandNoteAdd();

        result.NoteID = Modable.copyDeep(NoteID);
        result.Text = Modable.copyDeep(Text);

        return result;
    }

    private void mod(CommandNoteAdd original, CommandNoteAdd mod)
    {
        NoteID = Modable.mod(original.NoteID, mod.NoteID);
        Text = Modable.mod(original.Text, mod.Text);


    }

    public void mod(CommandNoteAdd modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandNoteAdd modCommand = modable as CommandNoteAdd;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
