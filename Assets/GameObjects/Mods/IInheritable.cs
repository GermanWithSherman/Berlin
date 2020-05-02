using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInheritable : IModable
{
    void inherit();

    bool isInheritanceResolved();

    
}

public static class Inheritable
{
    public static T Inherited<T>(T inheritable) where T : IInheritable
    {
        if (!inheritable.isInheritanceResolved())
            inheritable.inherit();
        return inheritable;

    }
}