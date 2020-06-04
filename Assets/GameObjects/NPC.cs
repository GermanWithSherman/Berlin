using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class NPC : Data, IInheritable, IModable, IModableAutofields
{
    [JsonIgnore]
    public string id;

    public string NameFirst;
    public string NameLast;


    public CText NameNick;
    public bool ShouldSerializeNameNick() => false;

    [JsonProperty("BirthDate")]
    public DateTime? birthDate;

    [JsonIgnore]
    public DateTime BirthDate
    {
        get => birthDate.GetValueOrDefault();
    }

    [JsonIgnore]
    public int age
    {
        get => GameManager.Instance.timeAgeYears(BirthDate);
        set => birthDate = GameManager.Instance.timeWithAge(value);
    }

    public string GenderVisible;

    public string BreastSize
    {
        get => breastVolumeToSize(BreastVolume);
        set
        {
            BreastVolume = breastSizeToVolume(value);
        }
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
    public int Height = 1700; //in mm
    public int PenisLength;
    public int Weight = 6000; //in g

    [JsonIgnore]
    public float BMI
    {
        get => (Weight/1000f)/(Mathf.Pow(Height/1000f,2));
        set => Weight = Mathf.RoundToInt(value * Mathf.Pow(Height / 1000f, 2)*1000);
    }

    [JsonProperty("Schedules")]
    public ModableDictionary<TimeFilters> SchedulesDict = new ModableDictionary<TimeFilters>();
    //public ModableDictionary<Schedule> SchedulesDict = new ModableDictionary<Schedule>();
    public bool ShouldSerializeSchedulesDict() => false;
    //[JsonIgnore]
    //public IEnumerable<TimeFilters> Schedules { get => SchedulesDict.Values; }
    //public IEnumerable<Schedule> Schedules { get => SchedulesDict.Values; }

    public string TemplateID;
    [JsonIgnore]
    public NPCTemplate Template
    {
        get => GameManager.Instance.NPCTemplateCache[TemplateID];
    }

    [JsonIgnore]
    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }

    public Conditional<string> TexturePath;
    public virtual bool ShouldSerializeTexturePath() => false;

    [JsonIgnore]
    public bool inheritanceResolved = false;


    private int breastSizeToVolume(string value)
    {
        switch (value)
        {
            case "None":
                return 0;
            default:
                return UnityEngine.Random.Range(100,1000);

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

    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "age":
                return age;
            case "genderVisible":
                return GenderVisible;
            case "nameFirst":
                return NameFirst;
            case "nameLast":
                return NameLast;
            case "nameNick":
                return CText.Text(NameNick);
            case "birthDate":
                return birthDate;
            case "height":
                return Height;
            case "texturePath":
                return TexturePath.value();
            case "known":
                return 1;
            case "weight":
                return Weight;
            case "BMI":
                return BMI;

        }
        return null;
    }

    protected override void set(string key, dynamic value)
    {
        setNPCData(key,value);
    }

    protected void setNPCData(string key, dynamic value)
    {
        switch (key)
        {
            case "age":
                age = value;
                break;
            case "genderVisible":
                GenderVisible = value;
                break;
            case "nameFirst":
                NameFirst = value;
                break;
            case "nameLast":
                NameLast = value;
                break;
            case "birthDate":
                birthDate = value;
                break;
            case "height":
                Height = (int)value;
                break;
            case "texturePath":
                TexturePath = new Conditional<string>((string)value,0,true);
                break;
            case "weight":
                Weight = (int)value;
                break;
            case "BMI":
                BMI = value;
                break;
        }
    }

    public void inherit() => templateApply();

    public void templateApply()
    {
        if(!inheritanceResolved)
            inherit(Template.generate());
        inheritanceResolved = true;
    }


    public void mod(NPC modNPC)
    {
        if (modNPC == null) return;
        mod(this, modNPC);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((NPC)modable);
    }

    private void mod(NPC original, NPC mod)
    {
        birthDate = Modable.mod(original.birthDate, mod.birthDate);

        NameFirst = Modable.mod(original.NameFirst, mod.NameFirst);

        NameLast = Modable.mod(original.NameLast, mod.NameLast);

        GenderVisible = Modable.mod(original.GenderVisible, mod.GenderVisible);

        SchedulesDict = Modable.mod(original.SchedulesDict, mod.SchedulesDict);

        TexturePath = Modable.mod(original.TexturePath, mod.TexturePath);

        NameNick = Modable.mod(original.NameNick, mod.NameNick);
        //TODO: etc.
    }

    public IModable copyDeep()
    {
        var result = new NPC();
        result.TemplateID = Modable.copyDeep(TemplateID);
        result.birthDate = Modable.copyDeep(birthDate);
        result.NameFirst = Modable.copyDeep(NameFirst);
        result.NameLast = Modable.copyDeep(NameLast);
        result.NameNick = Modable.copyDeep(NameNick);
        result.GenderVisible = Modable.copyDeep(GenderVisible);
        result.SchedulesDict = Modable.copyDeep(SchedulesDict);
        result.TexturePath = Modable.copyDeep(TexturePath);
        //TODO: etc.
        return result;
    }

    public void inherit(NPC parent)
    {
        if (parent == null)
            return;


        NPC parentCopy = Modable.copyDeep(parent);

        mod(parentCopy, this);

    }

    public bool isInheritanceResolved() => inheritanceResolved;

}

public class NPCComparer : IEqualityComparer<NPC>
{
    public bool Equals(NPC x, NPC y)
    {
        return (x.id == y.id);
    }

    public int GetHashCode(NPC obj)
    {
        //Check whether the object is null
        if (System.Object.ReferenceEquals(obj, null)) return 0;

        return obj.id.GetHashCode();
    }
}