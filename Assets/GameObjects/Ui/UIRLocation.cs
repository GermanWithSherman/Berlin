using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRLocation : MonoBehaviour
{
    private LocationConnection locationConnection;

    public UnityEngine.UI.Button Button;
    //public UnityEngine.UI.RawImage RawImage;
    public ImageAutosize Image;
    public UnityEngine.UI.Text Text;

    public void setRLocation(LocationConnection locationConnection)
    {
        GameData gameData = GameManager.Instance.GameData;

        this.locationConnection = locationConnection;

        if (!String.IsNullOrEmpty(locationConnection.Label))
            Text.text = locationConnection.Label;
        else
            Text.text = locationConnection.TargetLocation.Label.Text(gameData);

        Image.texture = locationConnection.Texture;

        Button.onClick.RemoveAllListeners();

        if (locationConnection.TargetLocation.isOpen())
        {
            Button.onClick.AddListener(() => locationConnection.execute());
        }
        else
        {
            Button.interactable = false;
        }
    }
}
