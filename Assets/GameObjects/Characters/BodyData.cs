using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class BodyData : Data, IModable, IModableAutofields
{
    public string GenderVisible;


    [JsonProperty("BirthDate")]
    public DateTime? _BirthDate;

    [JsonIgnore]
    public DateTime BirthDate
    {
        get => _BirthDate.GetValueOrDefault();
        set => _BirthDate = value;
    }

    [JsonIgnore]
    public int Age
    {
        get => GameManager.Instance.timeAgeYears(BirthDate);
        set => _BirthDate = GameManager.Instance.timeWithAge(value);
    }


    public int BreastVolume;
    public string HairColor;
    public int HairLength;
    public string HairStyle;
    public string HairBodyColor;
    public int HairBodyLength;
    public string HairPubicColor;
    public int HairPubicLength;
    public string HairPubicStyle;
    public int Height;
    public int PenisLength;
    public int Weight;

    

    [JsonIgnore]
    public float BMI
    {
        get => (Weight / 1000f) / (Mathf.Pow(Height / 1000f, 2));
        set => Weight = Mathf.RoundToInt(value * Mathf.Pow(Height / 1000f, 2) * 1000);
    }

    [JsonIgnore]
    public string BreastSize
    {
        get => breastVolumeToSize(BreastVolume);
        set
        {
            BreastVolume = breastSizeToVolume(value);
        }
    }


    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "Age": return Age;
            case "BirthDate": return BirthDate;
            case "BMI": return BMI;
            case "BreastSize": return BreastSize;
            case "BreastVolume": return BreastVolume;
            case "GenderVisible": return GenderVisible;
            case "HairColor": return HairColor;
            case "HairLength": return HairLength;
            case "HairStyle": return HairStyle;
            case "HairBodyColor": return HairBodyColor;
            case "HairBodyLength": return HairBodyLength;
            case "HairPubicColor": return HairPubicColor;
            case "HairPubicLength": return HairPubicLength;
            case "HairPubicStyle": return HairPubicStyle;
            case "Height": return Height;
            case "PenisLength": return PenisLength;
            case "Weight": return Weight;
        }
        return null;
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "Age": Age = (int)value; return;
            case "BirthDate": BirthDate = value; return;
            case "BMI": BMI = (float)value; return;
            case "BreastSize": BreastSize = (string)value; return;
            case "BreastVolume": BreastVolume = (int)value; return;
            case "GenderVisible": GenderVisible = (string)value; return;
            case "HairColor": HairColor = (string)value; return;
            case "HairLength": HairLength = (int)value; return;
            case "HairStyle": HairStyle = (string)value; return;
            case "HairBodyColor": HairBodyColor = (string)value; return;
            case "HairBodyLength": HairBodyLength = (int)value; return;
            case "HairPubicColor": HairPubicColor = (string)value; return;
            case "HairPubicLength": HairPubicLength = (int)value; return;
            case "HairPubicStyle": HairPubicStyle = (string)value; return;
            case "Height": Height = (int)value; return;
            case "PenisLength": PenisLength = (int)value; return;
            case "Weight": Weight = (int)value; return;
        }
    }


    private int breastSizeToVolume(string value)
    {
        switch (value)
        {
            case "None":
                return 0;
            default:
                return UnityEngine.Random.Range(100, 1000);

        }
    }

    private string breastVolumeToSize(int breastVolume)
    {
        switch (breastVolume)
        {
            case 0:
                return "None";
            default:
                return "C";
        }
    }


    

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }
}
