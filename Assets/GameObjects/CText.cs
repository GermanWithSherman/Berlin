﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[JsonConverter(typeof(CTextConverter))]
public class CText : IModable
{
    public static implicit operator string(CText cText) => Text(cText);

    public string Value; //Plain Text
    public ModableObjectSortedDictionary<CText> Values = new ModableObjectSortedDictionary<CText>();
    public string JoinWith = " ";

    [JsonIgnore]
    public Condition Condition
    {
        get => GameManager.Instance.ConditionCache[_condition];
    }
    [JsonProperty("Condition")]
    public string _condition = ""; //condition String


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

        foreach (CText cText in Values.Values)
            result.Add(cText.Text(gameData));

        var resultString = String.Join(JoinWith, result);

        return resultString;
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

        string pattern = @"{([^({}]+)(\([^{}]+\))?}";
        //string pattern = @"{(\>?[\w\.\(\),\\:,""]*)(?>\| (\w +))?}";
        
        string input = s;

        int pos = 0;

        foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            try
            {
                int length = match.Index - pos;
                result += input.Substring(pos, length);

                string dataString = match.Groups[1].Value;

                dynamic d;

                if(match.Groups.Count == 3 && !String.IsNullOrEmpty(match.Groups[2].Value))
                {
                    //Function
                    string functionID = match.Groups[1].Value;
                    string functionParametersString = match.Groups[2].Value.Substring(1, match.Groups[2].Value.Length - 2);

                    FunctionParameters functionParameters = new FunctionParameters(functionParametersString);

                    d = GameManager.Instance.FunctionsLibrary.FunctionExecute(functionID, functionParameters);
                }
                else
                {
                    d = gameData[match.Groups[1].Value];
                }

                //result += format(d, match.Groups[2].Value);

                result += d;

                /*if (match.Groups.Count > 3 && !String.IsNullOrEmpty(match.Groups[2].Value))
                    result += format(d, match.Groups[2].Value);
                else
                    result += d;*/

                /*if (dataString[0] == '>')
                {


                    int indexOfOpen = dataString.IndexOf('(');
                    int indexOfClose= dataString.LastIndexOf(')');

                    string functionID = dataString.Substring(1,indexOfOpen-1);
                    string functionParametersString = dataString.Substring(indexOfOpen+1, indexOfClose-indexOfOpen-1);

                    FunctionParameters functionParameters = new FunctionParameters(gameData, functionParametersString.Split(','));

                    dataString = GameManager.Instance.FunctionsLibrary.FunctionExecute(functionID, functionParameters);

                    d = dataString;
                }
                else
                {
                    d = gameData[match.Groups[1].Value];
                }*/

                /*if (match.Groups.Count > 2 && !String.IsNullOrEmpty(match.Groups[2].Value))
                    result += format(d, match.Groups[2].Value);
                else
                    result += d;*/

            }
            catch (Exception e)
            {
                result += "{ERROR: "+e.GetType()+" }";
                Debug.LogError(e);
            }
            pos = match.Index + match.Length;
        }

        result += input.Substring(pos);

        result = lineParse(result);

        return result;
    }

    private static string lineParse(string input)
    {
        string pattern = @"<(\w+)(?>=([\w\.]+))?>(.*?)</\1>";
        string result = String.Empty;
        int pos = 0;

        foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
        {
            try
            {
                int length = match.Index - pos;
                result += input.Substring(pos, length);

                string tagType = match.Groups[1].Value;
                string parameter = match.Groups[2].Value;
                string content = match.Groups[3].Value;
                switch (tagType)
                {
                    case ("speaker"):
                        NPC speaker = GameManager.Instance.GameData.NPCsActive[parameter];
                        if(speaker == null)
                            result += $"<i><b><color=#CCCCCC>{content}</color></b></i>";
                        else
                            result += $"<i><b><color={speaker.DialogueColor}><link=\"NPC:{parameter}\">{content}</link></color></b></i>";
                        break;
                    default:
                        result += $"<{tagType}>{lineParse(content)}</{tagType}>";
                        break;
                }
            }
            catch (Exception e)
            {
                result += "{ERROR: " + e.GetType() + " }";
            }
            pos = match.Index + match.Length;
        }

        result += input.Substring(pos);

        return result;
    }


    public void mod(CText modable)
    {
        JoinWith = Modable.mod(JoinWith, modable.JoinWith);
        Value = Modable.mod(Value, modable.Value);
        Values = Modable.mod(Values, modable.Values);
        _condition = Modable.mod(_condition, modable._condition);
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
        result.JoinWith = Modable.copyDeep(JoinWith);
        result.Value = Modable.copyDeep(Value);
        result.Values = Modable.copyDeep(Values);
        result._condition = Modable.copyDeep(_condition);
        return result;
    }

    public override string ToString()
    {
        return Text();
    }
}
