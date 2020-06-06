using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialog : MonoBehaviour{
    protected DialogServer _dialogServer;

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public abstract void setSettings(DialogSetting settings);

    public void setServer(DialogServer dialogServer)
    {
        _dialogServer = dialogServer;
    }
}

public abstract class Dialog<T> : Dialog where T : DialogSetting
{
    
    //protected IDictionary<string, string> _settings;

    protected DialogResult _data = new DialogResult();

    protected T _settings;


    private void resolve()
    {
        resolve(GameManager.Instance.GameData);
    }

    private void resolve(Data data)
    {
        foreach (KeyValuePair<string,string> target in _settings.Targets)
        {
            data[target.Value] = _data[target.Key];
        }
    }


    protected void submit()
    {
        gameObject.SetActive(false);
        //_dialogServer.dialogSubmit(this, data);
        resolve();
        _settings.onComplete.execute();
        Destroy(gameObject);
    }

}

public abstract class DialogSetting
{
    public CommandsCollection onComplete = new CommandsCollection();
    public ModableValueTypeHashDictionary<string> Targets = new ModableValueTypeHashDictionary<string>();
}
