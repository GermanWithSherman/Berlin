using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextExtended : MonoBehaviour
{
    private string _text = "";

    //public RawImage ImagePrefab;
    public ImageAutosize ImagePrefab;
    public int ImageHeight = 200;
    public TextMeshProUGUI TextPrefab;

    public string Text
    {
        get => _text;
        set => setText(value);
    }

    public void addText(string text)
    {
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
                ImageAutosize image = Instantiate(ImagePrefab, transform);
                image.texture = GameManager.Instance.TextureCache[result];
                image.PreferredHeight = ImageHeight;
                //image.SizeToHeight(ImageHeight);
                //image.MinHeight = ImageMinHeight;
                //
                /*RectTransform rectTransform = image.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(200,200);*/
                //LayoutElement layoutElement = image.GetComponent<LayoutElement>();
                //layoutElement.preferredHeight = image.texture.height;
                //layoutElement.preferredWidth = image.texture.width;
            }
            i++;
        }
    }

    private void setText(string text)
    {
        transform.childrenDestroyAll();

        addText(text);

    }
}
