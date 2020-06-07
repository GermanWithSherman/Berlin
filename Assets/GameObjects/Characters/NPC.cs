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

    [JsonProperty("Body")]
    public BodyData _BodyData;
    [JsonIgnore]
    public BodyData BodyData
    {
        get
        {
            if (_BodyData == null)
                _BodyData = new BodyData();
            return _BodyData;
        }
    }




    [JsonProperty("Schedules")]
    public ModableObjectHashDictionary<TimeFilters> SchedulesDict = new ModableObjectHashDictionary<TimeFilters>();
    public bool ShouldSerializeSchedulesDict() => false;

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




    protected override dynamic get(string key)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 1) { 
            switch (key)
            {
                case "Age":
                    return age;
                case "NameFirst":
                    return NameFirst;
                case "NameLast":
                    return NameLast;
                case "NameNick":
                    return CText.Text(NameNick);
                case "BirthDate":
                    return birthDate;
                case "TexturePath":
                    return TexturePath.value();
                case "Known":
                    return 1;

            }
        }
        else
        {
            switch (keyParts[0])
            {
                case "Body":
                    return BodyData[keyParts[1]];
            }
        }

        return null;
    }

    protected override void set(string key, dynamic value)
    {
        setNPCData(key,value);
    }

    protected void setNPCData(string key, dynamic value)
    {
        string[] keyParts = key.Split(new char[] { '.' }, 2);

        if (keyParts.Length == 1)
        {
            switch (key)
            {
                case "Age":
                    age = value;
                    break;
                case "NameFirst":
                    NameFirst = value;
                    break;
                case "NameLast":
                    NameLast = value;
                    break;
                case "BirthDate":
                    birthDate = value;
                    break;
                case "TexturePath":
                    TexturePath = new Conditional<string>((string)value,0,true);
                    break;
            }
        }
        else
        {
            switch (keyParts[0])
            {
                case "Body":
                    BodyData[keyParts[1]] = value; return;
            }
        }
    }

    public static NPC templateApply(NPC original)
    {
        NPCTemplate template = original.Template;
        NPC templateInstance = template.generate();

        return Modable.mod(templateInstance, original);
    }


    public void inherit() => templateApply();

    public void templateApply()
    {
        throw new NotImplementedException();
        /*if(!inheritanceResolved)
            inherit(Template.generate());
        inheritanceResolved = true;*/
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
        throw new NotImplementedException();
    }

    public IModable copyDeep()
    {
        throw new NotImplementedException();
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