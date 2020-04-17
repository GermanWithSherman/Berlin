using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void gameContinue() {
        GameManager.Instance.gameLoad();
        hide();
    }


    public void gameNew()
    {
        GameManager.Instance.gameNew();
        hide();
    }


    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }
}
