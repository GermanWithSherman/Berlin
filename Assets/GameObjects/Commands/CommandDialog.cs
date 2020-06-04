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

    public override IModable copyDeep()
    {
        var result = new CommandDialog();

        result.DialogID = Modable.copyDeep(DialogID);
        result.Settings = Modable.copyDeep(Settings);

        return result;
    }

    private void mod(CommandDialog original, CommandDialog mod)
    {
        DialogID = Modable.mod(original.DialogID, mod.DialogID);
        Settings = Modable.mod(original.Settings, mod.Settings);


    }

    public void mod(CommandDialog modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public override void mod(IModable modable)
    {
        CommandDialog modCommand = modable as CommandDialog;
        if (modCommand == null)
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod(modCommand);
    }
}
