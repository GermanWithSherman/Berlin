using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreenItem : MonoBehaviour
{
    public ImageAutosize ImageAutosize;
    public string Slot;

    public void UpdateCharacter(NPC npc)
    {
        ImageAutosize.gameObject.SetActive(true);
        PC pc = npc as PC;
        if (pc == null)
            ImageAutosize.gameObject.SetActive(false);
        else
        {
            Outfit outfit = pc.CurrentOutfit;
            Item item = outfit[Slot];
            if(item == null)
                ImageAutosize.gameObject.SetActive(false);
            else
                ImageAutosize.texture = item.Texture;
        }
    }
}
