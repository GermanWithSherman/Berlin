using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogResult : Data
{
    private Dictionary<string, dynamic> _dict = new Dictionary<string, dynamic>();

    protected override dynamic get(string key)
    {
        return _dict[key];
    }

    protected override void set(string key, dynamic value)
    {
        _dict[key] = value;
    }

    
}
