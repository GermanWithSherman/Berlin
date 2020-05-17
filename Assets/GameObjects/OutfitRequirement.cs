using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitRequirement : IModable
{
    public ModableStringList AllowedStyles = new ModableStringList();
    public ModableStringList ForbiddenStyles = new ModableStringList();

    [JsonProperty("Instruction")]
    private string _instruction = "";

    [JsonIgnore]
    public string Instruction
    {
        get
        {
            if (!String.IsNullOrEmpty(_instruction))
                return _instruction;

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

    public IModable copyDeep()
    {
        var result = new OutfitRequirement();

        result.AllowedStyles = Modable.copyDeep(AllowedStyles);
        result.ForbiddenStyles = Modable.copyDeep(ForbiddenStyles);

        return result;
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

    public void mod(IModable modable)
    {
        var modOR = (OutfitRequirement)modable;

        AllowedStyles = Modable.mod(AllowedStyles, modOR.AllowedStyles);
        ForbiddenStyles = Modable.mod(ForbiddenStyles, modOR.ForbiddenStyles);
    }
}
