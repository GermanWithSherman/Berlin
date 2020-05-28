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
}
