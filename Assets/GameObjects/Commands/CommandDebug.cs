using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDebug : Command
{

    public CText Message = new CText();

    public override void execute(Data data)
    {
        Debug.Log(Message.Text());
    }
}
