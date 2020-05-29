using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandNoteRemove : Command
{
    public string NoteID;

    public override void execute(Data data)
    {
        GameManager.Instance.noteRemove(NoteID);
    }

    public override IModable copyDeep()
    {
        var result = new CommandNoteRemove();

        result.NoteID = Modable.copyDeep(NoteID);

        return result;
    }

    private void mod(CommandNoteRemove original, CommandNoteRemove mod)
    {
        NoteID = Modable.mod(original.NoteID, mod.NoteID);


    }

    public void mod(CommandNoteRemove modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandNoteRemove modCommand = modable as CommandNoteRemove;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
