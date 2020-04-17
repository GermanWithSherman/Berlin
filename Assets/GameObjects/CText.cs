using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class CText
{
    public string V; //Plain Text

    public Dictionary<string, CText> D = new Dictionary<string, CText>();

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[C];
    }
    public string C = ""; //condition String

    public string Text(GameData gameData)
    {
        if (!Condition.evaluate(gameData))
            return "";

        List<string> result = new List<string>();

        if (!String.IsNullOrEmpty(V))
            result.Add(parse(V,gameData));

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

    private static string parse(string s, GameData gameData)
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
}
