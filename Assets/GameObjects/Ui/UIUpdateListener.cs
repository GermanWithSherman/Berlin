﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIUpdateListener : MonoBehaviour
{
    public abstract void uiUpdate(GameManager gameManager);

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }
}
