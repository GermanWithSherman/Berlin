using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextExtended : MonoBehaviour
{
    private string _text;

    public RawImage ImagePrefab;
    public TextMeshProUGUI TextPrefab;

    public string Text
    {
        get => _text;
        set => setText(value);
    }


    private void setText(string text)
    {
        transform.childrenDestroyAll();

        int i = 0;

        string pattern = @"<img>([^<]+)</img>";

        foreach (string result in Regex.Split(text, pattern))
        {
            if (i % 2 == 0)
            {
                if (!String.IsNullOrEmpty(result))
                {
                    TextMeshProUGUI uiText = Instantiate(TextPrefab, transform);
                    uiText.text = result;
                }
            }
            else
            {
                RawImage image = Instantiate(ImagePrefab, transform);
                image.texture = GameManager.Instance.TextureCache[result];
                LayoutElement layoutElement = image.GetComponent<LayoutElement>();
                layoutElement.preferredHeight = image.texture.height;
                layoutElement.preferredWidth = image.texture.width;
            }
            i++;
        }


    }
}
