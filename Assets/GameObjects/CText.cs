using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[JsonConverter(typeof(CTextConverter))]
public class CText : IModable
{
    public string Value; //Plain Text

    public ModableDictionary<CText> D = new ModableDictionary<CText>();

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[C];
    }
    public string C = ""; //condition String


    public CText() { }

    public CText(string s)
    {
        Value = s;
    }

    public string Text()
    {
        return Text(GameManager.Instance.GameData);
    }

    public static string Text(CText cText)
    {
        if (cText == null)
            return "";
        return cText.Text();
    }

    public string Text(Data gameData)
    {
        if (!Condition.evaluate(gameData))
            return "";

        List<string> result = new List<string>();

        if (!String.IsNullOrEmpty(Value))
            result.Add(parse(Value, gameData));

        foreach (CText cText in D.Values)
            result.Add(cText.Text(gameData));

        return String.Join(" ", result);
    }

    private static string format(dynamic data, string format)
    {
        switch (format.ToLower())
        {
            case "cap":
                return ((string)data).ToUpper();
        }
        return "FORMAT INVALID";
    }

    private static string parse(string s, Data gameData)
    {
        string result = String.Empty;
        string pattern = @"{([\w\.]*)(?>\|(\w+))?}";
        string input = s;

        int pos = 0;

        foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            int length = match.Index - pos;
            result += input.Substring(pos, length);

            if (match.Groups.Count > 2 && !String.IsNullOrEmpty(match.Groups[2].Value))
                result += format(gameData[match.Groups[1].Value], match.Groups[2].Value);
            else
                result += gameData[match.Groups[1].Value];
            pos = match.Index + match.Length;
            //match.Groups[1].Value, match.Index
        }

        result += input.Substring(pos);


        return result;
    }
 

    public void mod(CText modable)
    {
        Value = Modable.mod(Value, modable.Value);
        D = Modable.mod(D, modable.D);
        C = Modable.mod(C, modable.C);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((CText)modable);
        
    }

    public IModable copyDeep()
    {
        var result = new CText();
        result.Value = Modable.copyDeep(Value);
        result.D = Modable.copyDeep(D);
        result.C = Modable.copyDeep(C);
        return result;
    }
}
