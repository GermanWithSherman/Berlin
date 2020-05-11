using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        //return typeof(CText) == objectType;
        return true;
    }

    public override bool CanWrite => false;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        Command result;

        JObject jo = JObject.Load(reader);

        if (!Enum.TryParse((string)jo["Type"], out Command.Type commandType))
        {
            Debug.LogError($"CommandType {jo["Type"]} not recognized in {jo}");
            return new CommandNone();
        }

        switch (commandType)
        {
            case (Command.Type.Break):
                result = new CommandBreak();
                break;
            case (Command.Type.Conditional):
                result = new CommandConditional();
                break;
            case (Command.Type.Consume):
                result = new CommandConsume();
                break;
            case (Command.Type.Continue):
                result = new CommandContinue();
                break;
            case (Command.Type.Debug):
                result = new CommandDebug();
                break;
            case (Command.Type.Dialog):
                result = new CommandDialog();
                break;
            case (Command.Type.Event):
                result = new CommandEvent();
                break;
            case (Command.Type.EventEnd):
                result = new CommandEventEnd();
                break;
            case (Command.Type.Flush):
                result = new CommandFlush();
                break;
            case (Command.Type.GotoLocation):
                result = new CommandGotoLocation();
                break;
            case (Command.Type.Interrupt):
                result = new CommandInterrupt();
                break;
            case (Command.Type.ItemAdd):
                result = new CommandItemAdd();
                break;
            case (Command.Type.Outfit):
                result = new CommandOutfit();
                break;
            case (Command.Type.Pause):
                result = new CommandPause();
                break;
            case (Command.Type.Services):
                result = new CommandServices();
                break;
            case (Command.Type.Set):
                result = new CommandSet();
                break;
            case (Command.Type.Shop):
                result = new CommandShop();
                break;
            case (Command.Type.Sleep):
                result = new CommandSleep();
                break;
            case (Command.Type.TimePass):
                result = new CommandTimePass();
                break;
            case (Command.Type.None):
            default:
                result = new CommandNone();
                break;
        }

        serializer.Populate(jo.CreateReader(), result);

        return result;
        /*if (reader.TokenType == JsonToken.Null)
            return new CText();

        if (reader.TokenType == JsonToken.StartObject)
        {
            //https://stackoverflow.com/questions/35586987/json-net-custom-serialization-with-jsonconverter-how-to-get-the-default-beha
            existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
            serializer.Populate(reader, existingValue);
            return existingValue;
        }

        if (reader.TokenType == JsonToken.String)
            return new CText((string)reader.Value);

        */
        //throw new JsonSerializationException();


    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}