using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAutosize : MonoBehaviour, ILayoutGroup
{

    public RawImage Child;

    private int _preferredHeight = 200;
    public int PreferredHeight
    {
        get => _preferredHeight;
        set
        {
            _preferredHeight = value;
            LayoutElement layoutElement = GetComponent<LayoutElement>();
            layoutElement.preferredHeight = PreferredHeight;
        }
    }

    public Texture texture
    {
        get => Child?.texture;
        set
        {
            if (Child != null)
            {
                Child.texture = value;
                updateChild();
            }
        }
    }

    void Awake()
    {
        //_child = GetComponentInChildren<RawImage>(true);

        LayoutElement layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = PreferredHeight;

        //texture = GameManager.Instance.TextureCache["npc/school/staff/psychologist_horizontal.jpg"];
    }


    public void SetLayoutHorizontal()
    {
        if (Child != null && texture != null)
            updateChild();
    }

    public void SetLayoutVertical()
    {
        if(Child != null && texture !=null)
            updateChild();
    }

    private void updateChild()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        RectTransform childRectTransform = Child.GetComponent<RectTransform>();

        //Example:
        //Image: 400x300        600x400
        //Rect: 400x150         300x400
        //ratioWidth: 1         1
        //ratioHeight: 0.5      
        //ratio: 0.5
        //Result: 200x150
        float ratioHeight = rectTransform.rect.height / texture.height;
        float ratioWidth = rectTransform.rect.width / texture.width;

        float ratio = Mathf.Min(ratioHeight, ratioWidth);

        //float s = Mathf.Min(rectTransform.rect.height, rectTransform.rect.width);

        //childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, s);
        //childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, s);
        
        childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ratio * texture.height);
        childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ratio * texture.width);
    }
}
