using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Data
{
    [JsonIgnore]
    public string id;

    public string nameFirst;
    public string nameLast;

    public DateTime birthDate;

    [JsonIgnore]
    public int age
    {
        get => GameManager.Instance.timeAgeYears(birthDate);
        set => birthDate = GameManager.Instance.timeWithAge(value);
    }

    public string genderVisible = "m";

    public int height = 1700; //in mm
    public int weight = 6000; //in g

    [JsonIgnore]
    public float BMI
    {
        get => (weight/1000f)/(Mathf.Pow(height/1000f,2));
        set => weight = Mathf.RoundToInt(value * Mathf.Pow(height / 1000f, 2)*1000);
    }

    public Dictionary<string, Schedule> schedules = new Dictionary<string, Schedule>();
    [JsonIgnore]
    public IEnumerable<Schedule> Schedules { get => schedules.Values; }

    public string templateId;
    [JsonIgnore]
    public NPCTemplate Template
    {
        get => GameManager.Instance.NPCTemplateCache[templateId];
    }

    [JsonIgnore]
    public Texture Texture { get => GameManager.Instance.TextureCache[TexturePath]; }
    public Conditional<string> TexturePath;

    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "age":
                return age;
            case "genderVisible":
                return genderVisible;
            case "nameFirst":
                return nameFirst;
            case "nameLast":
                return nameLast;
            case "birthDate":
                return birthDate;
            case "height":
                return height;
            case "known":
                return 1;
            case "weight":
                return weight;
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
                genderVisible = value;
                break;
            case "nameFirst":
                nameFirst = value;
                break;
            case "nameLast":
                nameLast = value;
                break;
            case "birthDate":
                birthDate = value;
                break;
            case "height":
                height = (int)value;
                break;
            case "weight":
                weight = (int)value;
                break;
            case "BMI":
                BMI = value;
                break;
        }
    }


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