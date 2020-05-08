using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialog : MonoBehaviour
{
    private DialogServer _dialogServer;
    protected IDictionary<string, string> _settings;

    protected DialogResult data = new DialogResult();

    

    public void setServer(DialogServer dialogServer)
    {
        _dialogServer = dialogServer;
    }

    public virtual void setSettings(IDictionary<string, string> settings)
    {
        _settings = settings;
    }

    protected void submit()
    {
        gameObject.SetActive(false);
        _dialogServer.dialogSubmit(this, data);
        Destroy(gameObject);
    }

}
