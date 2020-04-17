using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;

    void Start()
    {
        inputField.onSubmit.AddListener(GameManager.Instance.console);
    }
}
