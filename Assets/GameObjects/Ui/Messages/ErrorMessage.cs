using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessage : MonoBehaviour
{
    public static ErrorMessage Instance;

    //public TMPro.TextMeshProUGUI Text;

    public ErrorDialog ErrorDialogPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.LogError("Error: multiple " + this + " in scene!"); }
    }

    /*void Start()
    {
        close();
    }

    public void close()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        show("");
    }*/

    public void show(string msg)
    {
        ErrorDialog dialog = Instantiate(ErrorDialogPrefab, transform);
        dialog.Text.text = msg;
        //gameObject.SetActive(true);
    }

    public static void Show(string msg)
    {
        Instance?.show(msg);
    }

}
