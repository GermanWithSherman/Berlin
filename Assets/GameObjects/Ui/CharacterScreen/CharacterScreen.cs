using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreen : MonoBehaviour
{
    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show(NPC npc)
    {
        gameObject.SetActive(true);
        BroadcastMessage("UpdateCharacter",npc);
    }
}
