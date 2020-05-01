using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitRequirement
{
    public List<string> AllowedStyles = new List<string>();
    public List<string> ForbiddenStyles = new List<string>();

    public string CustomInstruction;

    [JsonIgnore]
    public string Instruction
    {
        get
        {
            if (!String.IsNullOrEmpty(CustomInstruction))
                return CustomInstruction;

            string result = "";
            if (AllowedStyles.Count == 0 && ForbiddenStyles.Count == 0)
                result += "All Styles are allowed.";
            else
            {
                if (AllowedStyles.Count > 0)
                    result += $"Style must be one of {String.Join(",",AllowedStyles)}.";
                if (ForbiddenStyles.Count > 0)
                    result += $"Style must NOT be one of {String.Join(",", ForbiddenStyles)}.";
            }
            return result;
        }
    }

    public bool isValid(Outfit outfit)
    {
        return isValid(outfit.Style);
    }

    public bool isValid(string outfitType)
    {
        if(AllowedStyles.Count > 0)
        {
            if (AllowedStyles.Contains(outfitType))
                return true;
            return false;
        }

        if (ForbiddenStyles.Count > 0)
        {
            if (ForbiddenStyles.Contains(outfitType))
                return false;
            return true;
        }

        return true;
    }
}
