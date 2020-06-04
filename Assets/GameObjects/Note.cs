using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public DateTime CreationTime;
    public string Text;

    public Note() { }

    public Note(string text, DateTime creationTime)
    {
        CreationTime = creationTime;
        Text = text;
    }
}
