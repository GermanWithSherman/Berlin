using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWindow : MonoBehaviour
{
    public void hide() => gameObject.SetActive(false);
    public void show() => gameObject.SetActive(true);

    public Transform WidgetContainer;

    public EditWidgetText EditWidgetText;

    private EditWidgetText _e;

    public void confirm()
    {
        hide();

        Location l = new Location();
        SubLocation sl = new SubLocation();
        sl.TexturePath = new Conditional<string>( _e.getCurrentValue(),0,true);
        l.subLocations["main"] = sl;

        GameManager.Instance.LocationCache.SetEditData("start",l);


        GameManager.Instance.CacheLocationsReset();
        GameManager.Instance.uiUpdate();
    }

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

