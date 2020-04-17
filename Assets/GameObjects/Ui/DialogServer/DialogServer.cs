using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogServer : MonoBehaviour
{
    public delegate void DialogCallback(DialogResult dialogResult);

    public Dialog SexSelectDialogPrefab;

    public Transform canvas;

    private Dictionary<Dialog, DialogCallback> callbacks = new Dictionary<Dialog, DialogCallback>();

    private Dictionary<Dialog, DialogResolver> resolvers = new Dictionary<Dialog, DialogResolver>();



    private Dialog dialogInstantiate(Dialog dialog)
    {
        Dialog result = Instantiate(dialog, canvas);
        result.setServer(this);
        return result;
    }

    private Dialog dialogInstantiate(string dialogId)
    {
        return dialogInstantiate(dialogGet(dialogId));
    }

    public void dialogShow(string dialogId, DialogCallback callback)
    {
        Dialog dialog = dialogInstantiate(dialogId);
        callbacks[dialog] = callback;
    }

    public void dialogShow(string dialogId, DialogResolver resolver)
    {
        Dialog dialog = dialogInstantiate(dialogId);
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

    private Dialog dialogGet(string dialogId)
    {
        switch (dialogId)
        {
            case "SexSelect":
                return SexSelectDialogPrefab;
        }
        throw new System.NotImplementedException($"Dialog with id {dialogId} is not available");
    }
}
