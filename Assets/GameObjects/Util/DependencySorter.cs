using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencySorter
{
    public static L Sort<T, L>(IEnumerable<T> input) where T : IDependentObject<T> where L : IList<T>, new()
    {

        Dictionary<T, IList<T>> dependenciesLeft = new Dictionary<T, IList<T>>();
        //List<T> sortedObjects = new List<T>();
        L sortedObjects = new L();
        List<T> unsortedObjects = new List<T>();

        foreach (T dependentObject in input)
        {        
            dependenciesLeft[dependentObject] = dependentObject.getDependencies();
            unsortedObjects.Add(dependentObject);
        }

        while (unsortedObjects.Count > 0)
        {
            bool independentObjectFound = false;
            foreach (T unsortedObject in unsortedObjects)
            {
                IList<T> dependencies = dependenciesLeft[unsortedObject];
                if (dependencies.Count == 0)
                {
                    independentObjectFound = true;
                    sortedObjects.Add(unsortedObject);
                    unsortedObjects.Remove(unsortedObject);
                    dependenciesLeft.Remove(unsortedObject);

                    foreach (IList<T> dependenciesInOtherObject in dependenciesLeft.Values)
                    {
                        dependenciesInOtherObject.Remove(unsortedObject);
                    }
                    break;
                }
            }

            if (!independentObjectFound)
                throw new Exception("Cyclic dependency detected");
        }

        return sortedObjects;

    }
}
