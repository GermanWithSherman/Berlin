using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDependentObject { }

public interface IDependentObject<T>: IDependentObject where T: IDependentObject
{
    IList<T> getDependencies();
}
