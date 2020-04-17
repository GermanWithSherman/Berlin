using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelView: MonoBehaviour
{

    public Color titleActiveColor;
    public Color titleInactiveColor;

    public Transform titlesContainer;
    public Transform windowsContainer;

    public UIPanelViewTitle titlePreset;
    public UIPanelViewWindow windowPreset;

    private List<UIPanelViewTitle> titles = new List<UIPanelViewTitle>();
    private List<UIPanelViewWindow> windows =  new List<UIPanelViewWindow>();

    private void addCategory(IPanelData data)
    {
        UIPanelViewTitle title = Instantiate(titlePreset,titlesContainer);
        title.set(data,this,titles.Count);
        titles.Add(title);

        UIPanelViewWindow window = Instantiate(windowPreset, windowsContainer);
        window.setContent(data);
        windows.Add(window);
    }

    private void clear()
    {
        titles = new List<UIPanelViewTitle>();
        windows = new List<UIPanelViewWindow>();

        foreach (Transform child in titlesContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in windowsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void setActive(int index)
    {
        for(int i = 0; i < windows.Count; i++)
        {
            if (i == index)
            {
                //titlesContainer.GetChild(i).gameObject.SetActive(true);
                windows[i].gameObject.SetActive(true);
            }
            else
            {
                //titlesContainer.GetChild(i).gameObject.SetActive(false);
                windows[i].gameObject.SetActive(false);
            }
        }
        Debug.Log("Switch Tab: "+index.ToString());
    }

    public void setCategories(IEnumerable<IPanelData> dataSet)
    {
        clear();

        foreach (IPanelData data in dataSet)
        {
            addCategory(data);
        }
        setActive(0);
    }


    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void update()
    {
        foreach (UIPanelViewWindow window in windows)
            window.update();
    }
}
