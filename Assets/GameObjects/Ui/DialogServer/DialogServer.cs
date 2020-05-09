using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogServer : MonoBehaviour
{
    public delegate void DialogCallback(DialogResult dialogResult);

    public Dialog AlarmClockDialogPrefab;
    public Dialog SelectHorizontalPrefab;
    public Dialog SexSelectDialogPrefab;

    public Transform canvas;

    private Dictionary<Dialog, DialogCallback> callbacks = new Dictionary<Dialog, DialogCallback>();

    private Dictionary<Dialog, DialogResolver> resolvers = new Dictionary<Dialog, DialogResolver>();

    private Dialog dialogInstantiate(Dialog dialog, DialogSetting setting)
    {
        Dialog result = Instantiate(dialog, canvas);
        result.setSettings(setting);
        return result;
    }

    private Dialog dialogInstantiate(Dialog dialog, IDictionary<string, string> settings)
    {
        Dialog result = Instantiate(dialog, canvas);
        result.setServer(this);
        //result.setSettings(settings);
        return result;
    }

    private Dialog dialogInstantiate(string dialogId, IDictionary<string, string> settings)
    {
        return dialogInstantiate(dialogGet(dialogId), settings);
    }

    public void dialogShow(Dialog dialog, DialogSetting setting)
    {
        dialogInstantiate(dialog, setting);
    }

    public void dialogShow(string dialogId, DialogCallback callback, IDictionary<string,string> settings)
    {
        Dialog dialog = dialogInstantiate(dialogId, settings);
        callbacks[dialog] = callback;
    }

    public void dialogShow(string dialogId, DialogResolver resolver, IDictionary<string, string> settings)
    {
        Dialog dialog = dialogInstantiate(dialogId, settings);
        resolvers[dialog] = resolver;
    }

    public void dialogSubmit(Dialog dialog, DialogResult dialogResult)
    {
        if (resolvers.ContainsKey(dialog))
        {
            DialogResolver resolver = resolvers[dialog];
            resolver.resolve(dialogResult);
        }

        if (callbacks.ContainsKey(dialog))
        {
            DialogCallback callback = callbacks[dialog];
            callbacks.Remove(dialog);
            callback(dialogResult);
        }
    }

    public Dialog dialogGet(string dialogId)
    {
        switch (dialogId)
        {
            case "AlarmClock":
                return AlarmClockDialogPrefab;
            case "SelectHorizontal":
                return SelectHorizontalPrefab;
            case "SexSelect":
                return SexSelectDialogPrefab;
        }
        throw new System.NotImplementedException($"Dialog with id {dialogId} is not available");
    }
}
