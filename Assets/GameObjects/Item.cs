using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : IModable
{
    [JsonIgnore]
    public string id;

    public string Label;
    public string TexturePath;

    public int Price = 100;

    public string Slot = "Clothes";

    //public string Style = "Casual"; //Casual, Glamour, Dance, Sports, Swimming; not enum for easy modability
    public ModableStringList Style = new ModableStringList() { "Casual" };

    public int Height = 0;
    public int Skimpiness = 0;

    public string Gender = "fm"; //f, fm, m

    public Texture Texture
    {
        get => GameManager.Instance.TextureCache[TexturePath];
    }

    public IModable copyDeep()
    {
        var result = new Item();

        result.Label = Modable.copyDeep(Label);
        result.TexturePath = Modable.copyDeep(TexturePath);
        result.Price = Modable.copyDeep(Price);
        result.Slot = Modable.copyDeep(Slot);
        result.Style = Modable.copyDeep(Style);
        result.Skimpiness = Modable.copyDeep(Skimpiness);
        result.Gender = Modable.copyDeep(Gender);
        result.Height = Modable.copyDeep(Height);
        return result;
    }

    public dynamic getDynamic(string key)
    {
        switch (key)
        {
            case "TexturePath":
                return TexturePath;
        }

        return "";
    }

    private void mod(Item original, Item mod)
    {
        Label = Modable.mod(original.Label, mod.Label);
        TexturePath = Modable.mod(original.TexturePath, mod.TexturePath);
        Price = Modable.mod(original.Price, mod.Price);
        Slot = Modable.mod(original.Slot, mod.Slot);
        Style = Modable.mod(original.Style, mod.Style);
        Skimpiness = Modable.mod(original.Skimpiness, mod.Skimpiness);
        Gender = Modable.mod(original.Gender, mod.Gender);
        Height = Modable.mod(original.Height, mod.Height);
    }
    public void mod(Item modItem)
    {
        if (modItem == null) return;
        mod(this, modItem);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((Item)modable);
    }
}
