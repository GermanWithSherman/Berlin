using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditButton : MonoBehaviour
{
    public void editCurrentLocation()
    {
        SubLocation subLocation = GameManager.Instance.GameData.CurrentLocation;
        GameManager.Instance.EditWindow.show(subLocation);
    }
}
