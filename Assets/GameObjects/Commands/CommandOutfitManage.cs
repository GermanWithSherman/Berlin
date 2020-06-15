using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CommandOutfitManage : Command
{
    public enum ManageMode { Save, Load, RemoveOutfitAndItems }
    public string Key;
    public string Value;
    [JsonConverter(typeof(StringEnumConverter))]
    public ManageMode Mode;

    public override IModable copyDeep()
    {
        var result = new CommandOutfitManage();

        result.Key = Modable.copyDeep(Key);
        result.Value = Modable.copyDeep(Value);
        result.Mode = Mode;

        return result;
    }

    public override void execute(Data data)
    {
        switch( Mode){
            case ManageMode.Load:
                if (!GameManager.Instance.PC.trySetOutfit(Key))
                    throw new GameException($"Failed to set outfit to {Key}");
                break;
            case ManageMode.Save:
                if (!GameManager.Instance.PC.trySaveOutfit(Key))
                    throw new GameException($"Failed to save outfit to {Key}");
                break;
            case ManageMode.RemoveOutfitAndItems:
                if (!GameManager.Instance.PC.tryRemoveOutfitAndItems(Key))
                    throw new GameException($"Failed to RemoveOutfitAndItems of outfit {Key}");
                break;
        }
    }

    public override void mod(IModable modable)
    {
        CommandOutfitManage modCommand = modable as CommandOutfitManage;
        if (modCommand == null)
            throw new Exception("Type mismatch");

        Key = Modable.mod(Key, modCommand.Key);
        Value = Modable.mod(Value, modCommand.Value);
        //Mode = Mode; Why would one mod Mode?
    }
}

