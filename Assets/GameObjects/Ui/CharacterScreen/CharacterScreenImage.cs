using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreenImage : MonoBehaviour
{
    public ImageAutosize ImageAutosize;

    public void UpdateCharacter(NPC npc)
    {
        ImageAutosize.texture = npc.Texture;
    }
}
