using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWindow : MonoBehaviour
{
    public void hide() => gameObject.SetActive(false);
    public void show() => gameObject.SetActive(true);

    public void show(Location location)
    {
        show();
    }

    public void show(SubLocation subLocation)
    {
        show();

        WidgetContainer.childrenDestroyAll();

        _e = Instantiate(EditWidgetText,WidgetContainer);
        _e.setCurrentValue(subLocation.TexturePath.value());

        Canvas.ForceUpdateCanvases();
    }
}



public class Unterricht
{
    
    //Diesen Teil fassen SuS nicht an
    private void WichtigeFunktion()
    {
        //Sehr wichtiger Code
    }


    public void Beispielprogramm()
    {
        //Hier wird von SuS programmiert
    }
}