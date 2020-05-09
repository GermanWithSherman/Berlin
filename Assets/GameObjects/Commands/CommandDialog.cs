using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDialog : Command
{
    public string DialogID;
    public JToken Settings = new JObject();

    public override void execute(Data data)
    {
        Dialog dialog = GameManager.Instance.DialogServer.dialogGet(DialogID);

        System.Type dialogType = dialog.GetType().BaseType;
        System.Type[] genericTypes = dialogType.GetGenericArguments();

        //DialogSetting setting = Settings.ToObject <genericTypes[0]> ();
        DialogSetting setting = (DialogSetting)JsonConvert.DeserializeObject(Settings.ToString(), genericTypes[0]);

        GameManager.Instance.DialogServer.dialogShow(dialog, setting);
    }
}
