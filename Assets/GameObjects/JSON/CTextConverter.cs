using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTextConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        //return typeof(CText) == objectType;
        return true;
    }

    public override bool CanWrite => false;

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if(reader.TokenType == JsonToken.Null)
            return new CText();

        if(reader.TokenType == JsonToken.StartObject)
        {
            //https://stackoverflow.com/questions/35586987/json-net-custom-serialization-with-jsonconverter-how-to-get-the-default-beha
            existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
            serializer.Populate(reader, existingValue);
            return existingValue;
        }

        if (reader.TokenType == JsonToken.String)
            return new CText((string)reader.Value);


        throw new JsonSerializationException();


    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
