using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : Data
{
    [JsonProperty("StatusBarVisible")]
    private bool? _statusBarVisible;

    public bool isVisibleStatusHunger()
    {
        if (_statusBarVisible.HasValue)
            return _statusBarVisible.Value;
        return true;
    }

    public bool isVisibleStatusMoney()
    {
        if (_statusBarVisible.HasValue)
            return _statusBarVisible.Value;
        return true;
    }

    public bool isVisibleStatusSleep()
    {
        if (_statusBarVisible.HasValue)
            return _statusBarVisible.Value;
        return true;
    }

    public bool isVisibleStatusTime()
    {
        if (_statusBarVisible.HasValue)
            return _statusBarVisible.Value;
        return true;
    }

    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "isVisibleStatusTime":
                return isVisibleStatusTime();
        }
        return true;
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "isVisibleStatusBar":
                _statusBarVisible = value;
                return;
        }
    }
}
