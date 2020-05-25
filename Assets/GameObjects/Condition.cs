using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Condition : IModable
{
    private const string regex_identifier = "[a-zA-Z0-9'\\._\\-\\+\\(\\)\\[\\],\"\\{\\}:]+";
    private const string regex_operator = "[><=]{1,2}|!=|⊆";
    private const string regex_whitespace = "\\s*";

    private const string regex_string_operator = regex_whitespace + "(" + regex_identifier + ")" + regex_whitespace + "(" + regex_operator + ")" + regex_whitespace + "(" + regex_identifier + ")" + regex_whitespace;


    enum Modes { Plain, Equals, HEquals, LEquals, NEquals, Higher, Lower, ElementOf, AND, OR, XOR }
    enum Types { Field, Value, Condition, Object }

    private Types leftType;
    private Types rightType;

    private dynamic leftValue;
    private dynamic rightValue;

    private Modes mode;

    private string conditionString = "";

    public Condition(string condition)
    {
        conditionString = condition.Trim();

        if (System.String.IsNullOrEmpty(condition))
        {
            leftType = Types.Value;
            leftValue = true;
            rightType = Types.Value;
            rightValue = null;
            mode = Modes.Plain;
            return;
        }

        # region Logic Connection

        if (conditionString.StartsWith("("))
        {
            int bracketLevel = 1;
            int currentStringPosition = 1;
            int maxLoops = 1000; //TODO : Check

            while (maxLoops-- > 0)
            {
                int nextOpen = conditionString.IndexOf('(', currentStringPosition);
                int nextClose = conditionString.IndexOf(')', currentStringPosition);

                if (nextOpen == -1)
                {
                    conditionString = conditionString.Substring(1, conditionString.Length - 2);
                    break;
                }
                else if (nextClose == -1)
                {
                    throw new Exception("Condition Malformed");
                }
                else if (nextOpen < nextClose)
                {
                    bracketLevel++;
                    currentStringPosition = nextOpen + 1;
                }
                else
                {
                    bracketLevel--;
                    currentStringPosition = nextClose + 1;

                    if (bracketLevel == 0)
                    {
                        //We cant be at the end, or nextOpen would have been -1
                        //Therefore we have to look for AND or OR or XOR
                        string leftHand = conditionString.Substring(1,nextClose-1);
                        string rightHandWithOperator = conditionString.Substring(currentStringPosition).Trim();

                        if (rightHandWithOperator.StartsWith("AND"))
                        {
                            string rightHand = rightHandWithOperator.Substring(3);
                            mode = Modes.AND;
                            leftType = Types.Condition;
                            leftValue = new Condition(leftHand);
                            rightType = Types.Condition;
                            rightValue = new Condition(rightHand);
                            return;
                        }else if (rightHandWithOperator.StartsWith("OR"))
                        {
                            string rightHand = rightHandWithOperator.Substring(2);
                            mode = Modes.OR;
                            leftType = Types.Condition;
                            leftValue = new Condition(leftHand);
                            rightType = Types.Condition;
                            rightValue = new Condition(rightHand);
                            return;
                        }
                        else if (rightHandWithOperator.StartsWith("XOR"))
                        {
                            string rightHand = rightHandWithOperator.Substring(3);
                            mode = Modes.XOR;
                            leftType = Types.Condition;
                            leftValue = new Condition(leftHand);
                            rightType = Types.Condition;
                            rightValue = new Condition(rightHand);
                            return;
                        }
                        else
                        {
                            throw new Exception("Condition Malformed, expecting logical operator AND, OR or XOR");
                        }
                    }
                }
            }
        }

        #endregion

        string pattern = Condition.regex_string_operator;
        MatchCollection matches = Regex.Matches(conditionString, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);


        if (matches.Count > 0)
        {
            Match match = matches[0];
            GroupCollection groups = match.Groups;

            switch (groups[2].Value)
            {
                case ">=":
                case "=>":
                    mode = Modes.HEquals;
                    break;
                case "<=":
                case "=<":
                    mode = Modes.LEquals;
                    break;
                case "=":
                case "==":
                    mode = Modes.Equals;
                    break;
                case ">":
                    mode = Modes.Higher;
                    break;
                case "<":
                    mode = Modes.Lower;
                    break;
                case "!=":
                    mode = Modes.NEquals;
                    break;
                case "⊆":
                    mode = Modes.ElementOf;
                    break;
            }

            dynamic[] value = new dynamic[2];
            Types[] valueType = new Types[2];

            string[] stringValues = new string[2];
            stringValues[0] = groups[1].Value;
            stringValues[1] = groups[3].Value;

            for (int i = 0; i < stringValues.Length; i += 1)
            {
                if (stringValues[i] == "true")
                {
                    value[i] = true;
                    valueType[i] = Types.Value;
                    continue;
                }

                if (stringValues[i] == "false")
                {
                    value[i] = false;
                    valueType[i] = Types.Value;
                    continue;
                }

                if (stringValues[i] == "null")
                {
                    value[i] = null;
                    valueType[i] = Types.Value;
                    continue;
                }

                if (stringValues[i].Length >= 3 && stringValues[i][0] == '"')
                {
                    value[i] = stringValues[i].Substring(1, stringValues[i].Length - 2);
                    valueType[i] = Types.Value;
                    continue;
                }

                if(stringValues[i].Length >= 3 && stringValues[i][0] == '{')
                {
                    value[i] = stringValues[i];
                    valueType[i] = Types.Object;
                    continue;
                }

                long l = 0;
                if (long.TryParse(stringValues[i], out l))
                {
                    value[i] = l;
                    valueType[i] = Types.Value;
                    continue;
                }

                decimal d = 0.0M;
                if (decimal.TryParse(stringValues[i], out d))
                {
                    value[i] = d;
                    valueType[i] = Types.Value;
                    continue;
                }

                

                value[i] = stringValues[i];
                valueType[i] = Types.Field;

            }

            leftType = valueType[0];
            leftValue = value[0];
            rightType = valueType[1];
            rightValue = value[1];

            return;

        }

        /*leftType = Types.Field;
        leftValue = "PC.Name";

        rightType = Types.Value;
        rightValue = "Christian";

        mode = Modes.Equals;*/
    }

    private static bool eq(dynamic left, dynamic right)
    {
        if (left.GetType() != right.GetType())
        {
            if (isInt(left) && isInt(right))
                return eq((long)left, (long)right);

            Debug.LogWarning($"Comparing incompatible types {left.GetType()} and {right.GetType()}");
            return false;
        }

        return (left == right);
    }

    private static bool higher(dynamic left, dynamic right)
    {
        if (isNumber(left) && isNumber(right))
        {
            return (left > right);
        }
        Debug.LogWarning($"Comparing incompatible types {left.GetType()} and {right.GetType()}");
        return false;
    }

    private static bool lower(dynamic left, dynamic right)
    {
        if(isNumber(left) && isNumber(right))
        {
            return (left < right);
        }
        Debug.LogWarning($"Comparing incompatible types {left.GetType()} and {right.GetType()}");
        return false;
    }

    public bool evaluate(Data data)
    {
        bool result = false;

        try
        {
            dynamic left = leftValue;
            dynamic right = rightValue;
            if (leftType == Types.Field)
                left = data[left];

            if (rightType == Types.Field)
                right = data[right];

            switch (mode)
            {
                case Modes.AND:
                    result = (((Condition)leftValue).evaluate(data) && ((Condition)rightValue).evaluate(data));
                    break;
                case Modes.OR:
                    result = (((Condition)leftValue).evaluate(data) || ((Condition)rightValue).evaluate(data));
                    break;
                case Modes.XOR:
                    result = (((Condition)leftValue).evaluate(data) ^ ((Condition)rightValue).evaluate(data)); //TODO: Test
                    break;
                case Modes.Plain:
                    result = (bool)leftValue;
                    break;
                case Modes.Equals:
                    result = eq(left, right);
                    break;
                case Modes.Higher:
                    result = higher(left, right);
                    break;
                case Modes.HEquals:
                    result = (higher(left, right) || eq(left, right));
                    break;
                case Modes.Lower:
                    result = lower(left,right);
                    break;
                case Modes.LEquals:
                    result = (lower(left, right) || eq(left, right));
                    break;
                case Modes.NEquals:
                    result = !eq(left,right);
                    break;
                case Modes.ElementOf:
                    result = elementOf(left, right);
                    break;
                default:
                    throw new GameException($"Unhandled Condition Type {mode.ToString()}");
            }
        }
        catch (Exception e)
        {
            throw new GameException($"Failed to evaluate condition: {conditionString} ( {e} )");
            //Debug.LogError("Failed to evaluate condition: "+ conditionString+"("+e+")");
        }

        return result;
    }

    private static bool elementOf(dynamic left, dynamic right)
    {
        if(left is DateTime)
        {
            var jObject = JObject.Parse((string)right);
            TimeFilter timeFilter = jObject.ToObject<TimeFilter>();
            return timeFilter.isValid(left);
        }
        Debug.LogWarning($"Unknown Type of left in Condition.elementOf: {left.GetType()}");
        return false;
    }

    private static bool isNumber(dynamic v)
    {
        return v is sbyte
                || v is byte
                || v is short
                || v is ushort
                || v is int
                || v is uint
                || v is long
                || v is ulong
                || v is float
                || v is double
                || v is decimal;
    }

    private static bool isInt(dynamic v)
    {
        return v is sbyte
                || v is byte
                || v is short
                || v is ushort
                || v is int
                || v is uint
                || v is long
                || v is ulong;
    }

    public void mod(IModable modable)
    {
        throw new NotImplementedException();
    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
    }
}
