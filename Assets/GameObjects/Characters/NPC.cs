using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class NPC : Data, IInheritable, IModable, IModableAutofields
{
    [JsonIgnore]
    public string id;

    #region Names
    [JsonProperty("NameFirst")]
    public string _nameFirst;
    [JsonProperty("NameLast")]
    public string _nameLast;

    [JsonIgnore]
    public string NameFirst
    {
        get => _nameFirst;
        set
        {
            //if (String.IsNullOrEmpty(_nameFirstBorn))
            //    _nameFirstBorn = value;
            _nameFirst = value;
        }
    }
    [JsonIgnore]
    public string NameLast
    {
        get => _nameLast;
        set
        {
            //if (String.IsNullOrEmpty(_nameFirstBorn))
            //    _nameLastBorn = value;
            _nameLast = value;
        }
    }

    public CText NameNick;

    [JsonProperty("NameFirstBorn")]
    public string _nameFirstBorn;
    [JsonProperty("NameLastBorn")]
    public string _nameLastBorn;

    [JsonIgnore]
    public string NameFirstBorn
    {
        get
        {
            if (String.IsNullOrEmpty(_nameFirstBorn))
                return NameFirst;
            return _nameFirstBorn;
        }
        set => _nameFirstBorn = value;
    }
    [JsonIgnore]
    public string NameLastBorn
    {
        get
        {
            if (String.IsNullOrEmpty(_nameLastBorn))
                return NameLast;
            return _nameLastBorn;
        }
        set => _nameLastBorn = value;
    }

    #endregion

    private DataCache<NPC,string> _nameCached = new DataCache<NPC,string>(GameManager.Instance.FunctionsLibrary.npcName);
    [JsonIgnore]
    public string Name => _nameCached.getValue(this);

    //public bool ShouldSerializeNameNick() => false;

    [JsonProperty("DialogueColor")]
    public string _dialogueColor;

    [JsonIgnore]
    public string DialogueColor
    {
        get
        {
            if (String.IsNullOrWhiteSpace(_dialogueColor))
                return "black";
            switch (_dialogueColor)
            {
                case "Female":
                    return "#BB0000";
                case "Male":
                    return "#0000BB";
            }
            return _dialogueColor;
        }
        set
        {
            _dialogueColor = value;
        }
    }


    [JsonProperty("BirthDate")]
    public DateTime? _BirthDate;

    [JsonIgnore]
    public DateTime BirthDate
    {
        get => _BirthDate.GetValueOrDefault();
        set
        {
            if (!BodyData._BirthDate.HasValue)
                BodyData.BirthDate = value;
            BirthDate = value;
        }
    }

    [JsonIgnore]
    public int age
    {
        get => GameManager.Instance.timeAgeYears(BirthDate);
        set => _BirthDate = GameManager.Instance.timeWithAge(value);
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

    [JsonProperty("Realtionships")]
    public RelationshipData _RelationshipData;
    [JsonIgnore]
    public RelationshipData RelationshipData
    {
        get
        {
            if (_RelationshipData == null)
                _RelationshipData = new RelationshipData();
            return _RelationshipData;
        }
    }

    [JsonProperty("Social")]
    public SocialData _SocialData;
    [JsonIgnore]
    public SocialData SocialData
    {
        get
        {
            if (_SocialData == null)
                _SocialData = new SocialData();
            return _SocialData;
        }
    }




    [JsonProperty("Schedules")]
    public ModableObjectHashDictionary<TimeFilters> SchedulesDict = new ModableObjectHashDictionary<TimeFilters>();
    public bool ShouldSerializeSchedulesDict() => false;

    /*public string TemplateID;
    [JsonIgnore]
    public NPCTemplate Template
    {
        get => GameManager.Instance.NPCTemplateCache[TemplateID];
    }*/

    public ModableStringList TemplateIDs;

    [JsonIgnore]
    public IEnumerable<NPCTemplate> Templates
    {
        get
        {
            var result = new List<NPCTemplate>();

            if (TemplateIDs == null)
                return result;

            foreach (string templateID in TemplateIDs)
            {
                result.Add(GameManager.Instance.NPCTemplateCache[templateID]);
            }

            return result;
        }
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
                case "Body":
                    return BodyData;
                case "NameFirst":
                    return NameFirst;
                case "NameLast":
                    return NameLast;
                case "NameNick":
                    return CText.Text(NameNick);
                case "NameFirstBorn":
                    return NameFirstBorn;
                case "NameLastBorn":
                    return NameLastBorn;
                case "BirthDate":
                    return BirthDate;
                case "TexturePath":
                    return TexturePath.value();
                case "Known":
                    return 1;
                case "ID":
                    return id;
                case "Social":
                    return SocialData;

            }
        }
        else
        {
            switch (keyParts[0])
            {
                case "Body":
                    return BodyData[keyParts[1]];
                case "Relationships":
                    return RelationshipData[keyParts[1]];
                case "Social":
                    return SocialData[keyParts[1]];
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
                    age = (int)value;
                    break;
                case "Body":
                    JToken bodyJToken = value as JToken;
                    if (bodyJToken == null)
                        throw new Exception($"Body has to be set as JToken. Given: {value.GetType()}");
                    try
                    {
                        _BodyData = bodyJToken.ToObject<BodyData>();
                    }
                    catch
                    {
                        throw new Exception($"Failed to cast BodyData in NPC.set.Body");
                    }
                    break;
                case "NameFirst":
                    NameFirst = value;
                    break;
                case "NameLast":
                    NameLast = value;
                    break;
                case "NameNick":
                    NameNick = new CText(value);
                    break;
                case "NameFirstBorn":
                    NameFirstBorn = value;
                    break;
                case "NameLastBorn":
                    NameLastBorn = value;
                    break;
                case "BirthDate":
                    BirthDate = value;
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
                case "Social":
                    SocialData[keyParts[1]] = value; return;
            }
        }
    }

    public static NPC templateApply(NPC original)
    {
        /*
        NPCTemplate template = original.Template;
        NPC templateInstance = template.generate();

        return Modable.mod(templateInstance, original);*/

        IEnumerable<NPCTemplate> templates = original.Templates;

       // NPC result = original;

        NPCTemplate completeTemplate = new NPCTemplate();

        foreach (NPCTemplate template in templates)
        {
            //NPC templateInstance = template.generate();
            //result = Modable.mod(templateInstance, result);
            completeTemplate = Modable.mod(completeTemplate, template);
        }

        NPC templateInstance = completeTemplate.generate();

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