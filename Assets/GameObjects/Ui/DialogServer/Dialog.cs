using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    private DialogServer _dialogServer;

    protected DialogResult data = new DialogResult();

    

    public void setServer(DialogServer dialogServer)
    {
        _dialogServer = dialogServer;
    }

    protected void submit()
    {
        gameObject.SetActive(false);
        _dialogServer.dialogSubmit(this, data);
        Destroy(gameObject);
    }

}
