using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandServices : Command
{
    public string ServicePointID;
    public override void execute(Data data)
    {
        GameManager.Instance.servicepointShow(ServicePointID);
    }
}
