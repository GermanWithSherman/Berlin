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
    }
}
