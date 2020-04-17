using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipProvider : MonoBehaviour,
                                IPointerEnterHandler, 
                                IPointerExitHandler                                
{

    public string Text;
    private Tooltip tooltip;


    void Start()
    {
        tooltip = GameManager.Instance.Tooltip;
    }

    void Update()
    {
        tooltip.setPosition(Input.mousePosition,gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.show(Text,eventData.position,gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.hide(gameObject);
    }
}
