using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTextExtended : MonoBehaviour
{
    public TextExtended TextExtended;

    public Button Button
    {
        get => GetComponent<Button>();
    }

    public bool Enabled
    {
        get => Button.interactable;
        set => Button.interactable = value;
    }

    public string Text
    {
        get => TextExtended.Text;
        set => TextExtended.Text = value;
    }
}
