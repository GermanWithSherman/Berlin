using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialog : MonoBehaviour{
    protected DialogServer _dialogServer;

    public abstract void setSettings(DialogSetting settings);

    public void setServer(DialogServer dialogServer)
    {
        _dialogServer = dialogServer;
    }
}

public abstract class Dialog<T> : Dialog where T : DialogSetting
{
    
    //protected IDictionary<string, string> _settings;

    protected DialogResult data = new DialogResult();

    protected T _settings;




    /*public virtual void setSettings(IDictionary<string, string> settings)
    {
        _settings = settings;
    }*/

    /*public void setSetting(JObject jObject)
    {
        _settings = jObject.ToObject<T>();
    }*/

    

    protected void submit()
    {
        gameObject.SetActive(false);
        //_dialogServer.dialogSubmit(this, data);
        _settings.onComplete.execute();
        Destroy(gameObject);
    }

}

public abstract class DialogSetting
{
    public CommandsCollection onComplete = new CommandsCollection();
}
