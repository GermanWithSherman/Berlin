using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarHunger : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;

    public void set(long hunger)
    {
        decimal h = hunger / 1000000m;
        Text.text = h.ToString("P");
    }
}
