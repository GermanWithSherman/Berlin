using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStageConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (!(reader.Value is String))
            throw new Exception("Expecting string");
        return GameManager.Instance.EventGroupCache.EventStage((string)reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (!(value is EventStage))
            throw new Exception("EventStage expected");
        writer.WriteValue(((EventStage)value).id);
    }
}
