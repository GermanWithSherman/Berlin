using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextPrefab : MonoBehaviour//, IPointerEnterHandler
{

    public TextMeshProUGUI Tmpro;

    private Tooltip tooltip;

    private float _secondsAfterLastTooltipCheck = 0.0f;
    private bool _tooltipactive = false;

    void Start()
    {
        tooltip = GameManager.Instance.Tooltip;
    }

    void OnDisable()
    {
        tooltip.hide(gameObject);
    }

    void OnGUI()
    {
        _secondsAfterLastTooltipCheck += Time.deltaTime;

        if (_secondsAfterLastTooltipCheck < 0.2 && !_tooltipactive)
            return;

        _secondsAfterLastTooltipCheck = 0.0f;

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(Tmpro, Input.mousePosition, null);

        
        //Debug.Log(e.mousePosition + " : "+ Input.mousePosition);

        if (linkIndex != -1)
        { 
            TMP_LinkInfo linkInfo = Tmpro.textInfo.linkInfo[linkIndex];

            string[] infoParts = linkInfo.GetLinkID().Split(':');

            switch (infoParts[0])
            {
                case "NPC":
                    if (infoParts.Length != 2)
                        throw new System.Exception($"Invalid Format of NPC link");
                    NPC npc = GameManager.Instance.GameData.NPCsActive[infoParts[1]];
                    tooltip.show(npc.Name, Input.mousePosition,gameObject);
                    _tooltipactive = true;
                    return;
            }
        }

        if (_tooltipactive)
        {
            _tooltipactive = false;
            tooltip.hide(gameObject);
        }

    }
}
