using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelViewTitle : MonoBehaviour
{
    public UnityEngine.UI.Button Button;
    public TMPro.TextMeshProUGUI TextMeshProUGUI;
    public string Text { get => TextMeshProUGUI.text; set => TextMeshProUGUI.text = value; }

    private UIPanelView UIPanelView;
    private int tabIndex = 0;

    public void set(IPanelData panelData, UIPanelView panelView, int tabIndex)
    {
        Text = panelData.title();
        UIPanelView = panelView;
        this.tabIndex = tabIndex;

        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(buttonClick);
    }

    private void buttonClick()
    {
        UIPanelView.setActive(tabIndex);
    }
}
