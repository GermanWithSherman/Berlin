using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanelViewWindow : MonoBehaviour
{
    public abstract void setContent(IPanelData panelData);

    public abstract void update();
}
