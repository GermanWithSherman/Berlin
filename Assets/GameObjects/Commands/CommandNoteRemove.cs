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
}
