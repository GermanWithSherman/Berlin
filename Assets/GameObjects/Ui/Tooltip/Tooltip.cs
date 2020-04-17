using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private GameObject activeGameObject;

    public Vector3 offset;

    public TMPro.TextMeshProUGUI Text;

    public void hide(GameObject sender)
    {
        if (sender != activeGameObject)
            return;

        gameObject.SetActive(false);
    }

    public void setPosition(Vector3 position,GameObject sender)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (sender != activeGameObject)
            return;


        transform.position = position + offset;





    }

    public void show(string message, Vector3 position, GameObject sender)
    {
        activeGameObject = sender;
        Text.text = message;

        setPosition(position,sender);
        gameObject.SetActive(true);
    }
}
