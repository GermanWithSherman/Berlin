using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [JsonIgnore]
    public string id;

    public string Label;
    public string TexturePath;

    public int Price = 100;

    public string Slot = "Clothes";

    public string Type = "Casual"; //Casual, Glamour, Dance, Sports, Swimming

    public int Skimpiness = 0;

    public string Gender = "f"; //f, fm, m

    public Texture Texture
    {
        get => GameManager.Instance.TextureCache[TexturePath];
    }
}
