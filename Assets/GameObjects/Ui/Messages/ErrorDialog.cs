using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDialog : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Text;

    public void close()
    {
        gameObject.SetActive(false);
    }
}
