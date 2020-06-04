using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINote : MonoBehaviour
{
    public TMPro.TextMeshProUGUI UIText;

    private Note _note;

    public string Text
    {
        get => UIText.text;
        set
        {
            UIText.text = value;
        }
    }

    public void setNote(Note note)
    {
        _note = note;
        Text = _note.Text;
    }
}
